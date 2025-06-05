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
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float slashDuration = 0.7f;
    [SerializeField] private GameObject slashEffect;
    [SerializeField, Range(0f, 180f)] private float attackAngle = 60f;
    [SerializeField] private float rotationSpeed = 720f;

    private bool canAttack = true;
    private NearMovement nearMovement;

    private void Start()
    {
        nearMovement = GetComponent<NearMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;
        if (nearMovement != null)
        { nearMovement.CanRotate = false; }
            

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f, enemyLayer | LayerMask.GetMask("Floor")))
        {
            targetPoint = hit.point;
        }
        else
        {
            Debug.LogWarning("No se encontro lugar para atacar");
            if (nearMovement != null)
                nearMovement.CanRotate = true;
            canAttack = true;
            yield break;
        }

        Vector3 dir = targetPoint - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f)
        {
            if (nearMovement != null)
                nearMovement.CanRotate = true;
            canAttack = true;
            yield break;
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        if (slashEffect != null && attackHitPoint != null)
        {
            Quaternion spawnRotation = Quaternion.LookRotation(-transform.forward);
            var slash = Instantiate(slashEffect, attackHitPoint.position, spawnRotation);
            slash.transform.rotation = spawnRotation;
            Destroy(slash, slashDuration);
        }

        Collider[] hits = Physics.OverlapSphere(attackHitPoint.position, attackRange, enemyLayer);
        foreach (var h in hits)
        {
            Vector3 directionToTarget = (h.transform.position - transform.position);
            directionToTarget.y = 0f;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= attackAngle / 2f)
            {
                Enemy enemy = h.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    if (hitEffect != null)
                        Instantiate(hitEffect, h.ClosestPoint(attackHitPoint.position), Quaternion.identity);
                    Debug.Log($"Has golpeado a {enemy.name} por {damage} de daño.");
                }
            }
        }

        yield return new WaitForSeconds(attackCooldownTime);
        if (nearMovement != null)
            nearMovement.CanRotate = true;
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackHitPoint != null)
        {
            Gizmos.color = Color.red;
            DrawCircle(attackHitPoint.position, attackRange, Vector3.up);
        }
    }

    private void DrawCircle(Vector3 center, float radius, Vector3 normal, int segments = 64)
    {
        var rot = Quaternion.FromToRotation(Vector3.up, normal.normalized);
        Vector3 prev = center + rot * (Vector3.forward * radius);
        for (int i = 1; i <= segments; i++)
        {
            float angle = 360f * i / segments;
            Vector3 curr = center + rot * (Quaternion.Euler(0, angle, 0) * Vector3.forward * radius);
            Gizmos.DrawLine(prev, curr);
            prev = curr;
        }
    }

    public float AttackDamage { get => damage; set => damage = value; }
}
