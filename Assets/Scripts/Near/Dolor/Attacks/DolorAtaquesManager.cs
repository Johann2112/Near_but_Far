using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DolorAtaquesManager : MonoBehaviour
{
    #region Variables Generales
    [Header("Funciones Dolor")]
    [SerializeField] private bool isDeployed = false;
    [SerializeField] private bool canDeploy = true;


    [Header("Dolor, Dolor y particulas")]
    [SerializeField] private GameObject Dolor; //Dolor Grande
    [SerializeField] private GameObject DolorChill; //Dolor Pequeño
    [SerializeField] private ParticleSystem particles; //Particulas que rodean a Near (Jugador)

    [Header("Valores para invocacion de Dolor")]
    [SerializeField] private float areaRadius; //Radio de la zona donde se puede invocar a Dolor
    [SerializeField] private float areaY; //Altura de la zona donde se puede invocar a Dolor
    [SerializeField] private LayerMask floorLayer; //Capa del suelo donde se puede invocar a Dolor
    [SerializeField] private float spawnHeightOffset; //Altura extra a la que se invoca a Dolor
    #endregion

    #region Teclas y Valores Ataques

    #region Tecla Invocar a Dolor
    [Header("Tecla Invocar a Dolor")]
    [SerializeField] private KeyCode deployKey = KeyCode.Q; //Invocar a Dolor
    #endregion

    #region Teclas de Ataques de Dolor y Valores
    [Header("Ataques :")]

    [SerializeField] private bool canAttack = true; //Puede atacar o no (Importante)

    #region Teclas Ataque 1 Tour inquiétant
    [Header("Ataque 1 Tour inquiétant")]
    [SerializeField] private KeyCode attack1Key = KeyCode.E; //Ataque 1
    [SerializeField] private float attack1Damage; //Daño del ataque 1
    private float attack1Cooldown;
    [SerializeField] private float attack1CooldownMax; //Cooldown del ataque 1
    [SerializeField] private float attack1StocksUsed; //Cuantas unidades se usan para el ataque 1 
    [SerializeField] private float movementSpeed; //Velocidad de movimiento del ataque 1
    [SerializeField] private float attack1Radius; //Radio de ataque del ataque 1
    private bool attack1OnCooldown = false;
    [SerializeField] private Image attack1CooldownImage; //Imagen de cooldown del ataque 1
    #endregion

    #region Teclas Ataque 2 Danse Fatale
    [Header("Ataque 2 Danse Fatale")]
    [SerializeField] private KeyCode attack2Key = KeyCode.R; //Ataque 2
    [SerializeField] private float attack2Damage; //Daño del ataque 2
    private float attack2Cooldown;
    [SerializeField] private float attack2CooldownMax; //Cooldown del ataque 2
    [SerializeField] private float attack2StocksUsed; //Cuantas unidades se usan para el ataque 2
    [SerializeField] private float attack2AmountOfHits; //Cuantas veces golpea el ataque 2
    [SerializeField] private float attack2SpinSpeed; //Velocidad de giro del ataque 2
    [SerializeField] Vector3 boxCenter = Vector3.zero; //Centro del area de ataque del ataque 2 
    [SerializeField] Vector3 boxSize = new Vector3(1f, 1f, 1f); //Tamaño del area de ataque del ataque 2
    private bool attack2OnCooldown = false;
    [SerializeField] private Image attack2CooldownImage; //Imagen de cooldown del ataque 2
    #endregion

    #region Teclas Ataque 3 Coupure Mortelle
    [Header("Ataque 3 Coupure Mortelle")]
    [SerializeField] private KeyCode attack3Key = KeyCode.F; //Ataque 3
    [SerializeField] private float attack3Damage; //Daño del ataque 3
    private float attack3Cooldown;
    [SerializeField] private float attack3CooldownMax; //Cooldown del ataque 3
    [SerializeField] private float attack3StocksUsed; //Cuantas unidades se usan para el ataque 3
    [SerializeField] private float attack3ThrowingForce; //Fuerza de lanzamiento del ataque 3
    [SerializeField] private Vector3 boxCenter3 = Vector3.zero; //Centro del area de ataque del ataque 3
    [SerializeField] private Vector3 boxSize3 = new Vector3(1f, 1f, 1f); //Tamaño del area de ataque del ataque 3
    private bool attack3OnCooldown = false;
    [SerializeField] private Image attack3CooldownImage; //Imagen de cooldown del ataque 3
    #endregion

    #region Teclas Ataque 4 La mort à l'envers
    [Header("Ataque 4 La mort à l'envers")]
    [SerializeField] private KeyCode attack4Key = KeyCode.C; //Ataque 4
    [SerializeField] private float attack4Damage; //Daño del ataque 4
    private float attack4Cooldown;
    [SerializeField] private float attack4CooldownMax; //Cooldown del ataque 4
    [SerializeField] private float attack4StocksUsed; //Cuantas unidades se usan para el ataque 4
    [SerializeField] private float attack4PullingForce; //Fuerza de atracción del ataque 4
    [SerializeField] private Vector3 boxCenter4 = Vector3.zero; //Centro del area de ataque del ataque 4
    [SerializeField] private Vector3 boxSize4 = new Vector3(1f, 1f, 1f); //Tamaño del area de ataque del ataque 4
    private bool attack4OnCooldown = false;
    [SerializeField] private Image attack4CooldownImage; //Imagen de cooldown del ataque 4
    #endregion

    #endregion

    #region Teclas de Ataques de Dolor EX y Valores
    //[Header("Ataques EX :")]
    //[Header("Ataque 1 (Cercle de souffrance) [Tour inquiétant]")]
    //#region Teclas Ataque 1 (Cercle de souffrance) [Tour inquiétant]
    //[SerializeField] private float attack1exDamage; //Daño del ataque 1 EX
    //[SerializeField] private float movementSpeedEX; //Velocidad de movimiento del ataque 1 EX
    //[SerializeField] private float attack1exRadius; //Radio de ataque del ataque 1 EX
    //[SerializeField] private float attack1exStocksUsed; //Cuantas unidades se usan para el ataque 1 EX
    //[SerializeField] private float attack1exTime; //Tiempo que dura el ataque 1 EX
    //#endregion
    #endregion

    #region Debug
    [SerializeField] private KeyCode testKey = KeyCode.G; //Bajar unidades por que si
    [SerializeField] private float testNumber; //Cantidad de unidades a bajar por el testKey
    #endregion
    #endregion

    #region Codigos para funcionamiento
    [SerializeField] private DolorMundoDeAmorYDolorManager dolorManager;
    #endregion

    #region Funciones de apoyo
    public bool Deployed()
    {
        return isDeployed;
    }

    public void SetDeployed(bool state)
    {
        isDeployed = state;
        Debug.Log("isDeployed = " + isDeployed);
    }

    public void CanDeployDolor(bool state)
    {
        canDeploy = state;
        Debug.Log("canDeploy = " + canDeploy);
    }


    #endregion

    private void Start()
    {
        #region Start Dolor
        if (Dolor != null)
        {
            Dolor.SetActive(false);
        }
        #endregion

        #region Particulas
        particles.Play();
        #endregion

        #region Start Cooldown Images
        if (attack1CooldownImage != null)
        {
            attack1CooldownImage.fillAmount = 0;
        }
        if (attack2CooldownImage != null)
        {
            attack2CooldownImage.fillAmount = 0;
        }
        if (attack3CooldownImage != null)
        {
            attack3CooldownImage.fillAmount = 0;
        }
        if (attack4CooldownImage != null)
        {
            attack4CooldownImage.fillAmount = 0;
        }
        #endregion
    }

    private void Update()
    {

        //if (Input.GetKeyDown(deployKey) && Input.GetKeyDown(attack1Key) && isDeployed && !attack1OnCooldown && canAttack)
        //{
        //    Debug.Log("Ataque combinado 1");
        //    Attack1Ex();
        //}

        //if (Input.GetKeyDown(deployKey) && Input.GetKeyDown(attack2Key) && isDeployed && !attack2OnCooldown && canAttack)
        //{
        //    Debug.Log("Ataque combinado 2");
        //}

        //if (Input.GetKeyDown(deployKey) && Input.GetKeyDown(attack3Key) && isDeployed && !attack3OnCooldown && canAttack)
        //{
        //    Debug.Log("Ataque combinado 3");
        //}

        //if (Input.GetKeyDown(deployKey) && Input.GetKeyDown(attack4Key) && isDeployed && !attack4OnCooldown && canAttack)
        //{
        //    Debug.Log("Ataque combinado 4");
        //}

        #region Invocar a Dolor
        if (Input.GetKeyDown(deployKey) && Dolor.activeSelf && canAttack && !Input.GetKeyDown(attack1Key) && !Input.GetKeyDown(attack2Key) && !Input.GetKeyDown(attack3Key) && !Input.GetKeyDown(attack4Key))
        {
            Dolor.SetActive(false);
            DolorChill.SetActive(true);
            particles.Play();
            SetDeployed(false);

            if (dolorManager.UnidadesValor < dolorManager.UnidadesValorMax)
            {
                canDeploy = false;
                dolorManager.OnCd();
            }
        }
        else if (Input.GetKeyDown(deployKey) && canDeploy && canAttack && !Input.GetKeyDown(attack1Key) && !Input.GetKeyDown(attack2Key) && !Input.GetKeyDown(attack3Key) && !Input.GetKeyDown(attack4Key))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, floorLayer))
            {
                if (Vector3.Distance(transform.position, hit.point) <= areaRadius)
                {
                    Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + Mathf.Abs(spawnHeightOffset), hit.point.z);
                    Dolor.transform.rotation = Quaternion.identity;
                    Dolor.transform.position = spawnPos;

                    RaycastHit downHit;
                    if (Physics.Raycast(spawnPos, Vector3.down, out downHit, 10f, floorLayer))
                    {
                        Dolor.transform.position = downHit.point + Vector3.up * 0.1f;
                    }
                    else
                    {
                        Dolor.transform.position = spawnPos + Vector3.up * 0.1f;
                    }

                    Dolor.SetActive(true);
                    DolorChill.SetActive(false);
                    particles.Stop();
                    SetDeployed(true);
                }
                else
                {
                    Debug.Log("It didn't work");
                }
            }
        }
        #endregion

        #region Debug
        if (Input.GetKeyDown(testKey) && isDeployed)
        {
            dolorManager.UnidadesValor -= testNumber;


        }
        #endregion

        #region Ataques de Dolor

        #region Ataque 1 Tour inquiétant
        if (Input.GetKeyDown(attack1Key) && isDeployed && !attack1OnCooldown && canAttack && !Input.GetKeyDown(deployKey))
        {
            Ataque1();
        }

        if (attack1OnCooldown)
        {
            attack1Cooldown += Time.deltaTime;
            if (attack1CooldownImage != null)
            {
                attack1CooldownImage.fillAmount = attack1Cooldown / attack1CooldownMax;
            }


            if (attack1Cooldown >= attack1CooldownMax)
            {
                attack1Cooldown = attack1CooldownMax;
                attack1OnCooldown = false;

                if (attack1CooldownImage != null)
                {
                    attack1CooldownImage.fillAmount = 0;
                }

            }
        }
        #endregion

        #region Ataque 2 Danse Fatale
        if (Input.GetKeyDown(attack2Key) && isDeployed && !attack2OnCooldown && canAttack && !Input.GetKeyDown(deployKey))
        {
            Ataque2();
            
        }

        if (attack2OnCooldown)
        {
            attack2Cooldown += Time.deltaTime;
            if (attack2CooldownImage != null)
            {
                attack2CooldownImage.fillAmount = attack2Cooldown / attack2CooldownMax;
            }

            if (attack2Cooldown >= attack2CooldownMax)
            {
                attack2Cooldown = attack2CooldownMax;
                attack2OnCooldown = false;

                if (attack2CooldownImage != null)
                {
                    attack2CooldownImage.fillAmount = 0;
                }
            }
        }
        #endregion

        #region Ataque 3 Coupure Mortelle
        if (Input.GetKeyDown(attack3Key) && isDeployed && !attack3OnCooldown && canAttack && !Input.GetKeyDown(deployKey))
        {

            Ataque3();
            
        }

        if (attack3OnCooldown)
        {
            if (attack3CooldownImage != null)
            {
                attack3CooldownImage.fillAmount = attack3Cooldown / attack3CooldownMax;
            }

            attack3Cooldown += Time.deltaTime;

            if (attack3Cooldown >= attack3CooldownMax)
            {
                attack3Cooldown = attack3CooldownMax;
                attack3OnCooldown = false;

                if (attack3CooldownImage != null)
                {
                    attack3CooldownImage.fillAmount = 0;
                }
            }
        }
        #endregion

        #region Ataque 4 La mort à l'envers
        if (Input.GetKeyDown(attack4Key) && isDeployed && !attack4OnCooldown && canAttack && !Input.GetKeyDown(deployKey))
        {

            Ataque4();
            

        }

        if (attack4OnCooldown)
        {
            if (attack4CooldownImage != null)
            {
                attack4CooldownImage.fillAmount = attack4Cooldown / attack4CooldownMax;
            }

            attack4Cooldown += Time.deltaTime;
            if (attack4Cooldown >= attack4CooldownMax)
            {
                attack4Cooldown = attack4CooldownMax;
                attack4OnCooldown = false;

                if (attack4CooldownImage != null)
                {
                    attack4CooldownImage.fillAmount = 0;
                }

            }
        }
        #endregion




        #endregion
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        #region Invocacion de Dolor
        Vector3 areaCenter = new Vector3(transform.position.x, areaY, transform.position.z);
        Gizmos.color = Color.green;
        DrawCircle(areaCenter, areaRadius, Vector3.up);
        #endregion

        #region Ataque 1 Gizmos
        Gizmos.color = Color.red;
        DrawCircle(Dolor.transform.position, attack1Radius, Vector3.up);
        #endregion

        #region Ataque 2 Gizmos
        Gizmos.color = Color.blue;
        Matrix4x4 matrix = Matrix4x4.TRS(Dolor.transform.TransformPoint(boxCenter), Dolor.transform.rotation, boxSize);
        Gizmos.matrix = matrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        #endregion

        #region Ataque 3 Gizmos
        Gizmos.color = Color.yellow;
        Matrix4x4 matrix3 = Matrix4x4.TRS(Dolor.transform.TransformPoint(boxCenter3), Dolor.transform.rotation, boxSize3);
        Gizmos.matrix = matrix3;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        #endregion

        #region Ataque 4 Gizmos
        Gizmos.color = Color.gray;
        Matrix4x4 matrix4 = Matrix4x4.TRS(Dolor.transform.TransformPoint(boxCenter4), Dolor.transform.rotation, boxSize4);
        Gizmos.matrix = matrix4;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        #endregion

        //#region Ataque 1 Ex Gizmos
        //Gizmos.matrix = Matrix4x4.identity;  // **¡MUY IMPORTANTE: reset antes del círculo!**
        //Gizmos.color = Color.red;
        //DrawCircle(transform.position, attack1exRadius, Vector3.up);
        //#endregion

    }

    #endregion

    #region Funcionamiento Ataque 1
    private void Ataque1()
    {
        canAttack = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, floorLayer))
        {
            


            Vector3 targetPos = hit.point;
            targetPos.y = Dolor.transform.position.y;

            StartCoroutine(Ataque1Mov(targetPos));
        }
    }

    private IEnumerator Ataque1Mov(Vector3 targetPos)
    {
        Vector3 startPos = Dolor.transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / movementSpeed;
        float spinSpeed = 360f / duration;

        HashSet<Enemy> EnemyHit = new HashSet<Enemy>();

        while (Vector3.Distance(Dolor.transform.position, targetPos) > 0.1f)
        {
            Dolor.transform.position = Vector3.MoveTowards(Dolor.transform.position, targetPos, movementSpeed * Time.deltaTime);
            Dolor.transform.Rotate(0, spinSpeed * Time.deltaTime, 0, Space.Self);

            Collider[] colliders = Physics.OverlapSphere(Dolor.transform.position, attack1Radius);

            foreach (Collider col in colliders)
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null && !EnemyHit.Contains(enemy))
                {
                    enemy.TakeDamage(attack1Damage);
                    EnemyHit.Add(enemy);
                }
            }

            yield return null;


        }
        dolorManager.UnidadesValor -= attack1StocksUsed;
        attack1Cooldown = 0;
        attack1OnCooldown = true;
        canAttack = true;
    }
    #endregion

    #region Funcionamiento Ataque 2
    private void Ataque2()
    {
        canAttack = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 targetPos = new Vector3(hit.point.x, Dolor.transform.position.y, hit.point.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - Dolor.transform.position);
            StartCoroutine(Ataque2Rot(targetRotation));

        }
    }
    private IEnumerator Ataque2Rot(Quaternion targetRotation)
    {

        while (Quaternion.Angle(Dolor.transform.rotation, targetRotation) > 0.1f)
        {
            Dolor.transform.rotation = Quaternion.RotateTowards(Dolor.transform.rotation, targetRotation, attack2SpinSpeed * Time.deltaTime);
            yield return null;

        }

        Dolor.transform.rotation = targetRotation;
        StartCoroutine(Ataque2Rep(attack2AmountOfHits));
    }

    private IEnumerator Ataque2Rep(float repeats)
    {
        Vector3 center = Dolor.transform.TransformPoint(boxCenter3);

        for (int i = 0; i < repeats; i++)
        {
            Collider[] colliders = Physics.OverlapBox(center, boxSize * 0.5f, Dolor.transform.rotation);

            if (Dolor.activeSelf)
            {
                foreach (Collider col in colliders)
                {
                    Debug.Log("Detectado: " + col.gameObject.name);
                    Enemy enemy = col.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(attack2Damage);
                    }
                }
                Debug.Log("Ataque 2 número: " + i);
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                Debug.Log("Dolor no activo, no se puede atacar");
            }


        }
        dolorManager.UnidadesValor -= attack2StocksUsed;
        attack2Cooldown = 0;
        attack2OnCooldown = true;
        canAttack = true;
    }
    #endregion

    #region Funcionamiento Ataque 3
    private void Ataque3()
    {
        canAttack = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 targetPos = new Vector3(hit.point.x, Dolor.transform.position.y, hit.point.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - Dolor.transform.position);
            StartCoroutine(Ataque3Rot(targetRotation));
        }
    }

    private IEnumerator Ataque3Rot(Quaternion targetRotation)
    {
        while (Quaternion.Angle(Dolor.transform.rotation, targetRotation) > 0.1f)
        {
            Dolor.transform.rotation = Quaternion.RotateTowards(Dolor.transform.rotation, targetRotation, attack2SpinSpeed * Time.deltaTime);
            yield return null;
        }
        Dolor.transform.rotation = targetRotation;

        Vector3 center = Dolor.transform.TransformPoint(boxCenter);

        Collider[] colliders = Physics.OverlapBox(center, boxSize3 * 0.5f, Dolor.transform.rotation);

        foreach (Collider col in colliders)
        {
            Debug.Log("Detectado: " + col.gameObject.name + "Lanzando hacia Arriba");
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attack3Damage);
                enemy.Launch(attack3ThrowingForce);
                Debug.Log("Lanzando enemigo hacia arriba con " + attack3ThrowingForce + " de fuerza");
            }
        }
        dolorManager.UnidadesValor -= attack3StocksUsed;
        attack3Cooldown = 0;
        attack3OnCooldown = true;
        canAttack = true;
    }
    #endregion

    #region Funcionamiento Ataque 4
    private void Ataque4()
    {
        canAttack = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 targetPos = new Vector3(hit.point.x, Dolor.transform.position.y, hit.point.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - Dolor.transform.position);
            StartCoroutine(Ataque4Rot(targetRotation));
        }
    }

    private IEnumerator Ataque4Rot(Quaternion targetRotation)
    {
        while (Quaternion.Angle(Dolor.transform.rotation, targetRotation) > 0.1f)
        {
            Dolor.transform.rotation = Quaternion.RotateTowards(Dolor.transform.rotation, targetRotation, attack2SpinSpeed * Time.deltaTime);
            yield return null;
        }
        Dolor.transform.rotation = targetRotation;
        Vector3 center = Dolor.transform.TransformPoint(boxCenter);
        Collider[] colliders = Physics.OverlapBox(center, boxSize4 * 0.5f, Dolor.transform.rotation);
        foreach (Collider col in colliders)
        {
            Debug.Log("Detectado: " + col.gameObject.name + "Jalandolo hacia Dolor");
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attack4Damage);
                enemy.Pull(attack4PullingForce, Dolor);
            }
        }
        dolorManager.UnidadesValor -= attack4StocksUsed;
        attack4Cooldown = 0;
        attack4OnCooldown = true;
        canAttack = true;
    }
    #endregion


//    private void Attack1Ex()
//    {
//        canAttack = false;

//        Dolor.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

//        StartCoroutine(Attack1EX());
//    }

//    private IEnumerator Attack1EX()
//{
//    float time = 0f;
//    float angle = 0f;
//    Vector3 centro = transform.position;
//    HashSet<Enemy> enemiesHit = new HashSet<Enemy>();

//    while (time < attack1exTime)
//    {
//        time += Time.deltaTime;
//        angle += movementSpeedEX * Time.deltaTime;

//        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
//        Vector3 dir = rotation * Vector3.forward;

//        Vector3 currentPosition = centro + dir * attack1exRadius;
//        Dolor.transform.position = currentPosition;

//        Collider[] hits = Physics.OverlapSphere(currentPosition, 1f);
//        foreach (Collider hit in hits)
//        {
//            Enemy enemy = hit.GetComponent<Enemy>();
//            if (enemy != null && !enemiesHit.Contains(enemy))
//            {
//                enemy.TakeDamage(attack1exDamage);
//                enemiesHit.Add(enemy);
//            }
//        }

//        yield return null;
//    }

//    canAttack = true;
//    dolorManager.UnidadesValor -= attack1exStocksUsed;
//    attack1Cooldown = 0;
//    attack1OnCooldown = true;
//}

    private void DrawCircle(Vector3 center, float radius, Vector3 normal, int segmentos = 64)
    {
        Vector3 tangent = Vector3.Cross(normal, Vector3.right);
        if (tangent == Vector3.zero)
            tangent = Vector3.Cross(normal, Vector3.forward);

        tangent.Normalize();
        Vector3 bitangent = Vector3.Cross(normal, tangent);

        float angleStep = 360f / segmentos;
        Vector3 prevPoint = center + radius * tangent;

        for (int i = 1; i <= segmentos; i++)
        {
            float angleRad = Mathf.Deg2Rad * angleStep * i;
            Vector3 point = center + radius * (Mathf.Cos(angleRad) * tangent + Mathf.Sin(angleRad) * bitangent);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
