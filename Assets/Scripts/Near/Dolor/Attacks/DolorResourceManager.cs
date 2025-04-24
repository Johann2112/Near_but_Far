using TMPro;
using UnityEngine;

public class DolorResourceManager : MonoBehaviour
{
    public static DolorResourceManager Instance;

    [SerializeField] private float mundoActual = 13f;
    [SerializeField] private float mundoMax = 13f;
    [SerializeField] private float rechargeRate = 1f;
    [SerializeField] private TextMeshProUGUI mundoUI;

    private bool enCooldown = false;

    public float MundoActual => mundoActual;
    public float MundoMax => mundoMax;
    public bool EnCooldown => enCooldown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        // Si la UI no se asign� desde el inspector, la buscamos por tag.
        if (mundoUI == null)
            mundoUI = GameObject.FindWithTag("TextUnits").GetComponent<TextMeshProUGUI>();
        // Ajustamos la tasa de recarga seg�n el m�ximo (puedes ajustar este c�lculo seg�n tu dise�o)
        rechargeRate = mundoMax / 13f;
    }

    private void Update()
    {
        // Regeneramos el recurso siempre, independientemente del cooldown.
        if (mundoActual < mundoMax)
        {
            mundoActual += rechargeRate * Time.deltaTime;
            mundoActual = Mathf.Min(mundoActual, mundoMax);
        }

        // Si est� en cooldown, desactivarlo autom�ticamente al alcanzar el m�ximo.
        if (enCooldown && mundoActual >= mundoMax)
        {
            enCooldown = false;
        }

        // Actualizamos la UI.
        if (mundoUI != null)
            mundoUI.text = mundoActual.ToString("F0");
    }

    // Consumo de recursos para los ataques.
    public bool TryConsume(float cost)
    {
        if (mundoActual >= cost)
        {
            mundoActual -= cost;
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        return Mathf.Approximately(mundoActual, mundoMax);
    }

    // Se activa el cooldown por desinvocaci�n con recurso incompleto o por haber llegado a 0.
    public void EntrarEnCooldown()
    {
        enCooldown = true;
    }

    public void ResetearRecursos()
    {
        mundoActual = mundoMax;
    }
}
