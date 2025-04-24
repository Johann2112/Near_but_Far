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
        // Si la UI no se asignó desde el inspector, la buscamos por tag.
        if (mundoUI == null)
            mundoUI = GameObject.FindWithTag("TextUnits").GetComponent<TextMeshProUGUI>();
        // Ajustamos la tasa de recarga según el máximo (puedes ajustar este cálculo según tu diseño)
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

        // Si está en cooldown, desactivarlo automáticamente al alcanzar el máximo.
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

    // Se activa el cooldown por desinvocación con recurso incompleto o por haber llegado a 0.
    public void EntrarEnCooldown()
    {
        enCooldown = true;
    }

    public void ResetearRecursos()
    {
        mundoActual = mundoMax;
    }
}
