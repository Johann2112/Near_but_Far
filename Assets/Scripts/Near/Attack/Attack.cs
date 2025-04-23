using UnityEngine;

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
    [SerializeField] private float criticChance;
    [SerializeField] private float criticDamage;

    [Header("Efectos")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Animator animator;

    private AudioSource audioSource;
    private Transform targetEnemy;
    private PlayerController playerMovement;

    private bool isRotatingToAttack = false;
    private bool canAttack = true;

    private void Start()
    {
        playerMovement = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isRotatingToAttack && canAttack)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackHitPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;

            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToEnemy);

            if (angle <= attackAngle / 2f)
            {
                DoDamage(enemy.transform);
                StartCoroutine(AttackCooldown());
            }
            else
            {
                StartCoroutine(RotateToEnemyAndAttack(enemy.transform));
            }

            break;
        }
    }

    private System.Collections.IEnumerator RotateToEnemyAndAttack(Transform enemy)
    {
        isRotatingToAttack = true;
        canAttack = false;

        playerMovement.canMove = false;
        playerMovement.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;

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

        playerMovement.canMove = true;
        playerMovement.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        isRotatingToAttack = false;
    }

    private void DoDamage(Transform enemyTransform)
    {
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        if (enemy != null)
        {
            bool isCritic = Random.Range(0f, 1f) < criticChance;
            float finaldamage = damage;

            if (isCritic)
            {
                finaldamage *= criticDamage;
                Debug.Log($"¡Golpe crítico! Daño aumentado a {criticDamage}.");
            
            }

            enemy.TakeDamage(finaldamage);
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, enemyTransform.position, Quaternion.identity);
        }


        /* if (audioSource != null && attackSound != null)
        //     audioSource.PlayOneShot(attackSound);

        
        // if (animator != null)
        /    animator.SetTrigger("Attack");*/

        Debug.Log($"Ataque exitoso a {enemyTransform.name} con {damage} de daño.");
    }

    private System.Collections.IEnumerator AttackCooldown()
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
            Gizmos.DrawWireSphere(attackHitPoint.position, attackRange);
        }
    }
}
