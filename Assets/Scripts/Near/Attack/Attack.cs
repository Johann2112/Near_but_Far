using UnityEngine;
using System.Collections;
public class Attack : MonoBehaviour
{
    [Header("Parámetros de ataque")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Transform attackHitPoint;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldownTime = 1.2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField, Range(0f, 180f)] private float attackAngle = 60f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float hitSpeedToMove;
    [SerializeField] private GameObject slashEffect;


    [Header("Efectos")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Animator animator;

    private AudioSource audioSource;
    private Transform targetEnemy;
    private NearMovement nearMovement;

    private bool isRotatingToAttack = false;
    private bool canAttack = true;

    private void Start()
    {
        nearMovement = GetComponent<NearMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRotatingToAttack && canAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance <= attackRange)
                {
                    TryAttack(hit.transform);
                }
            }
        }
    }

    private void TryAttack(Transform enemy)
    {
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToEnemy);

        if (angle <= attackAngle / 2f)
        {
            DoDamage(enemy);
            StartCoroutine(AttackCooldown());
        }
        else
        {
            StartCoroutine(RotateToEnemyAndAttack(enemy));
        }
    }

    private IEnumerator RotateToEnemyAndAttack(Transform enemy)
    {
        isRotatingToAttack = true;
        canAttack = false;

        nearMovement.CanMove = false;

        Vector3 direction = (enemy.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        DoDamage(enemy);
        StartCoroutine(AttackCooldown());

        yield return new WaitForSeconds(hitSpeedToMove);

        nearMovement.CanMove = true;
        isRotatingToAttack = false;
    }

    private void DoDamage(Transform enemyTransform)
    {
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        if (enemy != null)
        {
            float finaldamage = damage;
            enemy.TakeDamage(finaldamage);
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, enemyTransform.position, Quaternion.identity);
        }

        if (slashEffect != null)
        {
            Vector3 spawnPosition = attackHitPoint.position;
            Quaternion spawnRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 180f, 0);

            GameObject effect = Instantiate(slashEffect, spawnPosition, spawnRotation);
            Destroy(effect, attackCooldownTime -0.5f);
        }

        Debug.Log($"Ataque exitoso a {enemyTransform.name} con {damage} de daño.");
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        if (attackHitPoint != null)
        {
            Gizmos.color = Color.red;
            DrawCircle(attackHitPoint.position, attackRange, Vector3.up);
        }
    }

    public float AttackDamage
    {
        get { return damage; }
        set { damage = value; }
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
}
