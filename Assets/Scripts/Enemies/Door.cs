using UnityEngine;

public class Door : Enemy
{
    [SerializeField] private int hitsCount;
    [SerializeField] private Dialogos dialogosSystem;
    [SerializeField] private int dialogoid;
    [SerializeField] private int hitsAmount;
    public override void TakeDamage(float amount)
    {

        hitsCount++;
        HitCountsDoor();
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

    private void HitCountsDoor()
    {

        if (hitsCount == hitsAmount)
        {
            dialogosSystem.TriggerDialogue(dialogoid);
        }
    }
}
