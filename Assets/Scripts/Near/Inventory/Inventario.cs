using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Inventario : MonoBehaviour
{

    #region Variables de los Fragmentos

    [SerializeField] private bool canUseItems = true;

    #region Rencor Fragmentado (Ataque+)
    [SerializeField] private float rencorFragmentadoAmount; //Cantidad de fragmetnos de rencor
    [SerializeField] private float valorAtaqueAunmentado; //Multiplo de Ataque de Near
    [SerializeField] private float tiempoRencorFragmentado; //Tiempo que dura el aumento de ataque
    [SerializeField] private Image rencorFragmentadoImage; //Cuadro de rencor fragmentado para el UI
    [SerializeField] private TextMeshProUGUI rencorFragmentadoText; //Texto de rencor fragmentado para el UI
    [SerializeField] private KeyCode rencorFragmentadoKey; //Tecla para usar fragmento de rencor
    #endregion

    #region Luto Fragmentado (Defensa+)
    [SerializeField] private float lutoFragmentadoAmount; //Cantidad de fragmentos de luto
    [SerializeField] private float valorDefensaAumentado; //Multiplo de Defensa de Near
    [SerializeField] private float tiempoLutoFragmentado; //Tiempo que dura el aumento de defensa
    [SerializeField] private Image lutoFragmentadoImage; //Cuadro de luto fragmentado para el UI
    [SerializeField] private TextMeshProUGUI lutoFragmentadoText; //Texto de luto fragmentado para el UI
    [SerializeField] private KeyCode lutoFragmentadoKey; //Tecla para usar fragmento de luto
    #endregion

    #region Desesperacion Fragmentada (Velocidad+)
    [SerializeField] private float desesperacionFragmentadaAmount; //Cantidad de fragmentos de desesperacion
    [SerializeField] private float valorVelocidadAumentada; //Multiplo de Velocidad de Near
    [SerializeField] private float tiempoDesesperacionFragmentada; //Tiempo que dura el aumento de velocidad
    [SerializeField] private Image desesperacionFragmentadaImage; //Cuadro de desesperacion fragmentada para el UI
    [SerializeField] private TextMeshProUGUI desesperacionFragmentadaText; //Texto de desesperacion fragmentada para el UI
    [SerializeField] private KeyCode desesperacionFragmentadaKey; //Tecla para usar fragmento de desesperacion
    #endregion
    #endregion

    #region Refencias de Scripts
    [SerializeField] private Attack attack; //Referencia al script de ataque

    [SerializeField] private NearLife nearLife; //Referencia al script de vida (defensa)

    [SerializeField] private NearMovement nearMovement; //Referencia al script de movimiento
    #endregion

    private void Start()
    {
        #region Inicio Fragmentos (Image)
        if (rencorFragmentadoImage != null)
        {
            rencorFragmentadoImage.fillAmount = 1f;
        }

        if (lutoFragmentadoImage != null)
        {
            lutoFragmentadoImage.fillAmount = 1f;
        }

        if (desesperacionFragmentadaImage != null)
        {
            desesperacionFragmentadaImage.fillAmount = 1f;
        }
        #endregion

        #region Inicio Fragmentos (TextMeshProUGUI)
        if (rencorFragmentadoText != null)
        {
            rencorFragmentadoText.text = rencorFragmentadoAmount.ToString();
        }

        if (lutoFragmentadoText != null)
        {
            lutoFragmentadoText.text = lutoFragmentadoAmount.ToString();
        }

        if (desesperacionFragmentadaText != null)
        {
            desesperacionFragmentadaText.text = desesperacionFragmentadaAmount.ToString();
        }
        #endregion
    }

    private void Update()
    {
        #region Uso de Rencor Fragmentado (Ataque+)
        if (rencorFragmentadoImage != null)
        {
            if (rencorFragmentadoAmount >= 1)
            {
                rencorFragmentadoImage.fillAmount = 0f;
            }

        }
        
        if (Input.GetKeyDown(rencorFragmentadoKey) && rencorFragmentadoAmount > 0 && canUseItems)
        {
            rencorFragmentadoAmount--;
            UseRencorFragmentado();
            rencorFragmentadoImage.fillAmount = tiempoRencorFragmentado;
        }
        #endregion

        #region Uso de Luto Fragmentado (Defensa+)
        if (lutoFragmentadoImage != null)
        {
            if (lutoFragmentadoAmount >= 1)
            {
                lutoFragmentadoImage.fillAmount = 0f;
            }
        }

        if (Input.GetKeyDown(lutoFragmentadoKey) && lutoFragmentadoAmount > 0 && canUseItems)
        {
            lutoFragmentadoAmount--;
            UseLutoFragmentado();
            lutoFragmentadoImage.fillAmount = tiempoLutoFragmentado;
        }
        #endregion

        #region Uso de Desesperacion Fragmentada (Velocidad+)
        if (desesperacionFragmentadaImage != null)
        {
            if (desesperacionFragmentadaAmount >= 1)
            {
                desesperacionFragmentadaImage.fillAmount = 0f;
            }
        }

        if (Input.GetKeyDown(desesperacionFragmentadaKey) && desesperacionFragmentadaAmount > 0 && canUseItems)
        {
            desesperacionFragmentadaAmount--;
            UseDesesperacionFragmentada();
            desesperacionFragmentadaImage.fillAmount = tiempoDesesperacionFragmentada;
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Obtencion de Rencor Fragmentado
        if (other.CompareTag("Frag1"))
        {
            rencorFragmentadoAmount++;

            Destroy(other.gameObject);
        }
        #endregion

        #region Obtencion de Luto Fragmentado
        if (other.CompareTag("Frag2"))
        {
            lutoFragmentadoAmount++;
            Destroy(other.gameObject);
        }
        #endregion

        #region Obtencion de Desesperacion Fragmentada
        if (other.CompareTag("Frag3"))
        {
            desesperacionFragmentadaAmount++;
            Destroy(other.gameObject);
        }
        #endregion
    }

    #region Funcionamiento de Rencor Fragmentado
    private void UseRencorFragmentado()
    {
        canUseItems = false;

        float OriginalDamage = attack.AttackDamage;

        float ActualDamage; ActualDamage = attack.AttackDamage *= valorAtaqueAunmentado;

        StartCoroutine(UpdateFillAmount(rencorFragmentadoImage, tiempoRencorFragmentado));

        StartCoroutine(UseRencor(tiempoRencorFragmentado, ActualDamage, OriginalDamage));
    
}

    private IEnumerator UseRencor(float time, float damage, float originalDamage)
    {
        attack.AttackDamage = damage;

        yield return new WaitForSeconds(time);

        attack.AttackDamage = originalDamage;

        canUseItems = true;

    }
    #endregion

    #region Funcionamiento de Luto Fragmentado
    private void UseLutoFragmentado()
    {
        canUseItems = false;

        float OriginalDefense = nearLife.Defense;

        float ActualDefense;

        ActualDefense = nearLife.Defense *= valorDefensaAumentado;

        StartCoroutine(UseLuto(tiempoLutoFragmentado, ActualDefense, OriginalDefense));

        StartCoroutine(UpdateFillAmount(lutoFragmentadoImage, tiempoLutoFragmentado));
    }

    private IEnumerator UseLuto(float time, float defence, float originalDefence)
    {
        canUseItems = false;

        nearLife.Defense = defence;

        yield return new WaitForSeconds(time);

        nearLife.Defense = originalDefence;

        canUseItems = true;
    }
    #endregion

    #region Uso de Desesperacion Fragmentada
    private void UseDesesperacionFragmentada()
    {
        canUseItems = false;
        float OriginalSpeed = nearMovement.MoveSpeed;
        float OriginalDashSpeed = nearMovement.DashSpeed;

        float ActualDashSpeed;
        float ActualSpeed;

        ActualSpeed = nearMovement.MoveSpeed *= valorVelocidadAumentada;
        ActualDashSpeed = nearMovement.DashSpeed *= valorVelocidadAumentada;

        StartCoroutine(UsoDesesperacion(tiempoDesesperacionFragmentada, ActualSpeed, OriginalSpeed, ActualDashSpeed, OriginalDashSpeed));
        StartCoroutine(UpdateFillAmount(desesperacionFragmentadaImage, tiempoDesesperacionFragmentada));
    }

    private IEnumerator UsoDesesperacion(float time, float speed, float originalSpeed, float dashSpeed, float originalDashSpeed)
    {
        nearMovement.MoveSpeed = speed;
        nearMovement.DashSpeed = dashSpeed;

        yield return new WaitForSeconds(time);

        nearMovement.DashSpeed = originalDashSpeed;
        nearMovement.MoveSpeed = originalSpeed;
        canUseItems = true;

    }
    #endregion

    #region Funciones Utiles
    private IEnumerator UpdateFillAmount(Image image, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = 1f - (elapsedTime / duration);
            yield return null;
        }

        image.fillAmount = 1f;
    }
    #endregion
}
