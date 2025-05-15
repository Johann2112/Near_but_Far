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
    

    protected bool isCheckingGround = false;
    protected bool isBeingPulled = false;
    [SerializeField] private bool isLaunching;


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

    private void OnCollisionEnter(Collision collision)
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
}
