using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject textPrefab;

    public void TakeDamage(float amount)
    {


        health -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Salud restante: {health}");
        if (textPrefab)
        {
            ShowFloatingText(amount);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void ShowFloatingText(float amount)
    {

        GameObject floatingTextInstance = Instantiate(textPrefab, transform.position, Quaternion.identity);

        TextMeshPro textComponent = floatingTextInstance.GetComponent<TextMeshPro>();

        if (textComponent != null)
        {
            textComponent.text = amount.ToString();
        }


    }
}
