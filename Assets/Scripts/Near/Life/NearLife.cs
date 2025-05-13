using UnityEngine;
using UnityEngine.UI;

public class NearLife : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float lifeMax;
    [SerializeField] private float defence = 1f;

    [SerializeField] private Slider lifeSlider;

    private void Start()
    {
        lifeMax = life;
        if (lifeSlider != null)
        {
            lifeSlider.maxValue = lifeMax;
            lifeSlider.value = life;
        }
    }

    public void TakeDamage(float amount)
    {
        float finalAmount = amount / defence;
        life -= finalAmount;

        if (lifeSlider != null)
        {
            lifeSlider.value = life;
        }

        if (life <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        // Implementar la logica de muerte
        Debug.Log("Near ha muerto");
    }

    public float Defense
    {
        get { return defence; }
        set { defence = value; }

    }
}
