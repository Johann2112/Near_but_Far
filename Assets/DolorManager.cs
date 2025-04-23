using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DolorManager : MonoBehaviour
{
    [SerializeField] private float MundoDeAmorYDolor = 13f;

    [Header("Ataque 1 Giro Ominoso")]
    [SerializeField] private float attack1Cost = 1f;
    [SerializeField] private float cooldown1 = 1f;
    private float attack1Timer = 0f;

    [Header("Ataque 2 Danza Fatal")]
    [SerializeField] private float attack2Cost = 1f;
    [SerializeField] private float cooldown2 = 1f;
    private float attack2Timer = 0f;

    [Header("Ataque 3 Corte Funesto")]
    [SerializeField] private float attack3Cost = 1f;
    [SerializeField] private float cooldown3 = 1f;
    private float attack3Timer = 0f;

    [Header("Ataque 4 Muerte a la inversa")]
    [SerializeField] private float attack4Cost = 1f;
    [SerializeField] private float cooldown4 = 1f;
    private float attack4Timer = 0f;




    [SerializeField] private TextMeshProUGUI MundoDeAmorYDolorUI;

    
    
   
    

    private void Start()
    {
        MundoDeAmorYDolorUI.text = MundoDeAmorYDolor.ToString();
    }

    private void Update()
    {
        attack1Timer -= Time.deltaTime;
        attack2Timer -= Time.deltaTime;
        attack3Timer -= Time.deltaTime;
        attack4Timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Attack1();
            attack1Timer = cooldown1;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Attack2();
            attack2Timer = cooldown2;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Attack3();
            attack3Timer = cooldown3;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Attack4();
            attack4Timer = cooldown4;
        }
    }

    private void Attack1() //Pirueta
    {
        if (MundoDeAmorYDolor >= attack1Cost)
        {
            MundoDeAmorYDolor -= attack1Cost;
            MundoDeAmorYDolorUI.text = MundoDeAmorYDolor.ToString();
            // Implement the attack logic here
        }
    }


    private void Attack2() //Cortes rapidos en area pequeña
    {
        if (MundoDeAmorYDolor >= attack2Cost)
        {
            MundoDeAmorYDolor -= attack2Cost;
            MundoDeAmorYDolorUI.text = MundoDeAmorYDolor.ToString();
            // Implement the attack logic here
        }
    }
    private void Attack3() //Corte en un area grande
    {
        if (MundoDeAmorYDolor >= attack3Cost)
        {
            MundoDeAmorYDolor -= attack3Cost;
            MundoDeAmorYDolorUI.text = MundoDeAmorYDolor.ToString();
            // Implement the attack logic here
        }
    }
    private void Attack4() //Corte que atrae a los enemigos
    {
        if (MundoDeAmorYDolor >= attack4Cost)
        {
            MundoDeAmorYDolor -= attack4Cost;
            MundoDeAmorYDolorUI.text = MundoDeAmorYDolor.ToString();
            // Implement the attack logic here
        }
    }
}
