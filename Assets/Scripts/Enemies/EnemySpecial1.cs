using UnityEngine;
using System.Collections;

public class EnemySpecial1 : Enemy
{
    [SerializeField] protected float chargeSpeed = 10f;
    [SerializeField] protected float chargeDuration = 2f;
    [SerializeField] protected float chargeCooldown = 3f;
    [SerializeField] protected float chargeDistance = 10f;
    [SerializeField] protected float chargeWaitTime = 0.5f;
    [SerializeField] protected float postCrashRecoveryTime = 1.5f;

    protected bool isCharging = false;
    protected bool isRecovering = false;
    protected Vector3 chargeTarget;
    protected Coroutine chargeRoutine;

    private void Update()
    {
        if (!canMove || isCharging || isRecovering) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= visionRange)
        {
            if (chargeRoutine == null)
                chargeRoutine = StartCoroutine(ChargeSequence());
        }
    }

    private IEnumerator ChargeSequence()
    {
        canMove = false;

        chargeTarget = player.position;

        Vector3 direction = (chargeTarget - transform.position).normalized;
        direction.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        yield return new WaitForSeconds(chargeWaitTime);

        isCharging = true;
        float elapsedTime = 0f;

        rb.isKinematic = false;

        while (elapsedTime < chargeDuration)
        {
            rb.MovePosition(transform.position + direction * chargeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StopCharging();
    }

    private void StopCharging()
    {
        isCharging = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        StartCoroutine(ChargeCooldown());
    }

    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        canMove = true;
        chargeRoutine = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (!isCharging) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            NearLife playerHealth = collision.gameObject.GetComponent<NearLife>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            Debug.Log("Jugador golpeado por embestida.");
            StopCharging();
        }
        else if (collision.gameObject.CompareTag("Objeto"))
        {
            Debug.Log("Colisión con objeto. Enemigo en cd");
            isRecovering = true;
            isCharging = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            if (chargeRoutine != null)
            {
                StopCoroutine(chargeRoutine);
                chargeRoutine = null;
            }

            StartCoroutine(RecoverAfterCrash());
        }
    }

    private IEnumerator RecoverAfterCrash()
    {
        yield return new WaitForSeconds(postCrashRecoveryTime);
        isRecovering = false;
        canMove = true;
    }
}