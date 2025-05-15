using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NearLife : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float lifeMax;
    [SerializeField] private float defence = 1f;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Color textColor;
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
        ShowFloatingText(finalAmount,textColor);
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
        SceneManager.LoadScene("FP");
        Debug.Log("Near ha muerto");
    }

    public float Defense
    {
        get { return defence; }
        set { defence = value; }

    }

    private void ShowFloatingText(float amount, Color color)
    {
        GameObject floatingTextInstance = Instantiate(textPrefab, transform.position, Quaternion.identity);
        TextMeshPro textComponent = floatingTextInstance.GetComponent<TextMeshPro>();

        if (textComponent != null)
        {
            textComponent.text = amount.ToString();
            textComponent.color = color;
        }
    }
}
