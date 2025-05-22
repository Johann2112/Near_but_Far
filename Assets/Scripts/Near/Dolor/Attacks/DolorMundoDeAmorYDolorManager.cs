using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;
using TMPro;

public class DolorMundoDeAmorYDolorManager : MonoBehaviour
{
    [SerializeField] private float Unidades;
    [SerializeField] private float UnidadesMax;
    [SerializeField] private float Recarga;

    [SerializeField] private TextMeshProUGUI UnidadesText;
    private bool Max;
    private bool OnCooldown = false;


    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Color color1; //Color Inicial
    [SerializeField] private Color color2; //Color Secundario
    [SerializeField] private Color color3; //Color Terciario
    private bool isFlashing = false;
    //[SerializeField] private GameObject UI;
    [SerializeField] private GameObject Dolor;
    [SerializeField] private GameObject DolorChill;
    [SerializeField] private GameObject Player;

    private DolorAtaquesManager dolorAtaquesManager;

    private void Start()
    {
        UnidadesMax = Unidades;

        if (Player != null)
        {
            dolorAtaquesManager = Player.GetComponent<DolorAtaquesManager>();
        }
    }

    private void Update()
    {
        UnidadesText.text = Unidades.ToString("0");

        if (Dolor != null && Dolor.activeInHierarchy)
        {
            if (dolorAtaquesManager != null)
            {
                dolorAtaquesManager.SetDeployed(true);
            }

            OnCooldown = false;
        }
        else
        {

            dolorAtaquesManager.SetDeployed(false);
            if (OnCooldown)
            {
                dolorAtaquesManager.CanDeployDolor(false);
                Unidades += Time.deltaTime * Recarga;

                if (Unidades >= UnidadesMax)
                {
                    Unidades = UnidadesMax;
                    OnCooldown = false;

                    if (dolorAtaquesManager != null)
                    {
                        dolorAtaquesManager.CanDeployDolor(true);
                    }

                    StartCoroutine(FlashWhite());
                }
            }
            else
            {
                dolorAtaquesManager.CanDeployDolor(true);
            }
        }

        if (particles != null && !isFlashing)
        {
            float porcentaje = Unidades / UnidadesMax;
            Color colorActual = Color.Lerp(color1, color2, porcentaje);
            var colorModule = particles.main;
            colorModule.startColor = colorActual;
        }


    }

    public void OnCd()
    {

        OnCooldown = true;

    }

    public float UnidadesValor
    {
        get { return Unidades; }
        set
        {
            Unidades = Mathf.Clamp(value, 0, UnidadesMax);

            if (Unidades <= 0)
            {
                Debug.Log("Unidades 0");

                if (dolorAtaquesManager != null)
                {
                    StartCoroutine(WaitAndDisableDolor());
                }
                particles.Play();
            }
        }
    }

    public float UnidadesValorMax
    {

        get { return UnidadesMax; }

    }

    private IEnumerator FlashWhite()
    {
        Debug.Log("lUZ");
        isFlashing = true;
        var colorModule = particles.main;
        Color originalColor = colorModule.startColor.color;
        colorModule.startColor = color3;
        yield return new WaitForSeconds(1f);
        isFlashing = false;
        colorModule.startColor = originalColor;

    }

    private IEnumerator WaitAndDisableDolor()
    {
        yield return new WaitForSeconds(1.5f);

        Dolor.SetActive(false);
        DolorChill.SetActive(false);
        dolorAtaquesManager.SetDeployed(false);
        dolorAtaquesManager.CanDeployDolor(false);
        OnCd();
    }
}
