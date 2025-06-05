using UnityEngine;
using TMPro;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float armor = 0f;
    [SerializeField] protected GameObject deathEffect;
    [SerializeField] protected GameObject textPrefab;
    [SerializeField] protected float criticChance;
    [SerializeField] protected float criticDamage;
    [SerializeField] protected Color textColor;
    [SerializeField] protected Color criticTextColor;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundCheckDistance = 0.2f;
    [SerializeField] protected bool canMove = true;
    protected bool isCheckingGround = false;
    protected bool isBeingPulled = false;
    [SerializeField] protected bool isLaunching;

    [SerializeField] protected float visionRange = 10f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float attackRange = 1.5f;

    [SerializeField] protected float attackCooldownTime = 1.5f;
    [SerializeField] protected float attackDamage = 15f;
    [SerializeField] protected float attackAngle = 120f;
    [SerializeField] protected Transform attackHitPoint;
    [SerializeField] protected GameObject slashEffect;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected float hitSpeedToMove = 0.5f;

    protected bool canAttack = true;
    protected bool isRotatingToAttack = false;
    protected Transform player;

    public virtual void TakeDamage(float amount)
    {
        bool isCritic = Random.Range(0f, 1f) < criticChance;

        float finalDamage = amount;
        Color textDisplayColor = textColor;
        bool isCriticText = false;

        if (isCritic)
        {
            finalDamage *= criticDamage;
            Debug.Log($"¡Golpe crítico! Daño aumentado a {finalDamage}.");
            textDisplayColor = criticTextColor;
            isCriticText = true;
        }

        finalDamage *= (1f - armor);
        health -= finalDamage;

        Debug.Log($"{gameObject.name} recibió {finalDamage} de daño. Salud restante: {health}");

        if (textPrefab)
        {
            ShowFloatingText(finalDamage, textDisplayColor, isCriticText);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    protected void ShowFloatingText(float amount, Color color, bool isCritic)
    {
        GameObject floatingTextInstance = Instantiate(textPrefab, transform.position, Quaternion.identity);
        TextMeshPro textComponent = floatingTextInstance.GetComponent<TextMeshPro>();

        if (textComponent != null)
        {
            textComponent.text = amount.ToString();
            textComponent.color = color;
        }
    }

    public virtual void Launch(float amount)
    {
        rb.isKinematic = false;
        isLaunching = true;
        transform.position += Vector3.up * 0.1f;
        StartCoroutine(DelayedLaunch(amount));
    }

    private IEnumerator DelayedLaunch(float amount)
    {
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.05f);
        rb.AddForce(Vector3.up * amount, ForceMode.Impulse);
        yield return new WaitForSeconds(1.0f);
        isLaunching = false;
    }

    public virtual void Pull(float pullForce, GameObject where)
    {
        StartCoroutine(PullCoroutine(pullForce, where));
    }

    private IEnumerator PullCoroutine(float pullForce, GameObject where)
    {
        isBeingPulled = true;
        rb.isKinematic = false;

        Vector3 direction = (where.transform.position - transform.position).normalized;
        rb.AddForce(direction * pullForce, ForceMode.Impulse);

        float timer = 0f;
        while (Vector3.Distance(transform.position, where.transform.position) > 0.2f && timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        isBeingPulled = false;
        StartCoroutine(enumerato());
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (rb.isKinematic) return;

        if (!isBeingPulled && !isLaunching && collision.gameObject.layer == 7)
        {
            if (IsGrounded())
            {
                StartCoroutine(GroundChecker());
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Debug.Log("Tocó el suelo, se activa isKinematic");
            }
            else
            {
                Debug.Log("Colisión con suelo, pero aún en el aire");
            }
        }
    }

    private IEnumerator GroundChecker()
    {
        yield return new WaitForSeconds(0.1f);

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.isKinematic = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private IEnumerator enumerato()
    {
        yield return new WaitForSeconds(1f);
        rb.isKinematic = true;


    }

    private void Update()
    {
        if (!canMove || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                FollowPlayer();
            }
            else
            {
                TryAttack(player);
            }
        }
    }



    protected virtual void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void TryAttack(Transform target)
    {
        if (!canAttack || isRotatingToAttack)
        {
            return;

        }

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle <= attackAngle / 2f)
        {
            DoDamage(target);
            StartCoroutine(AttackCooldown());
        }
        else
        {
            StartCoroutine(RotateToTargetAndAttack(target));
        }
    }

    private IEnumerator RotateToTargetAndAttack(Transform target)
    {
        isRotatingToAttack = true;
        canMove = false;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        DoDamage(target);
        StartCoroutine(AttackCooldown());

        yield return new WaitForSeconds(hitSpeedToMove);

        canMove = true;
        isRotatingToAttack = false;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }


    private void DoDamage(Transform target)
    {
        NearLife targetPlayer = target.GetComponent<NearLife>();
        if (targetPlayer != null)
        {
            targetPlayer.TakeDamage(attackDamage);
        }

        if (slashEffect != null)
        {
            Instantiate(slashEffect, target.position, Quaternion.identity);
        }

        if (hitEffect != null && attackHitPoint != null)
        {
            Vector3 spawnPosition = attackHitPoint.position;
            Quaternion spawnRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 180f, 0);
            GameObject effect = Instantiate(hitEffect, spawnPosition, spawnRotation);
            Destroy(effect, attackCooldownTime - 0.5f);
        }

        Debug.Log($"Enemigo atacó a {target.name} con {attackDamage} de daño.");
    }


    private void DrawCircle(Vector3 center, float radius, Vector3 normal, int segmentos = 64)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal.normalized);
        Vector3 prevPoint = center + rot * (Vector3.forward * radius);

        for (int i = 1; i <= segmentos; i++)
        {
            float angle = 360f * i / segmentos;
            Vector3 newPoint = center + rot * (Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }


    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
            DrawCircle(transform.position, visionRange, Vector3.up);
        }
        

        Gizmos.color = Color.yellow;
        DrawCircle(transform.position, attackRange, Vector3.up);
    }
}
