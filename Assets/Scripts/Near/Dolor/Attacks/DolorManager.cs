using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolorManager : MonoBehaviour
{
    [Header("Ataque 1 - Giro Ominoso")]
    [SerializeField] private float attack1Cost = 1f;
    [SerializeField] private float cooldown1 = 1f;
    private float attack1Timer = 0f;
    [SerializeField] private float movementSpeed = 10f;            
    [SerializeField] private float attack1Damage = 5f;               
    [SerializeField] private LayerMask floorLayer;                 
    [SerializeField] private LayerMask enemyLayer;                 
    [SerializeField] private float collisionRadius = 0.5f;         
    [SerializeField] private ParticleSystem swordParticleFront;   
    [SerializeField] private ParticleSystem swordParticleBack; 

    [Header("Ataque 2 - Danza Fatal")]
    [SerializeField] private float attack2Cost = 1f;
    [SerializeField] private float cooldown2 = 2f;
    private float attack2Timer = 0f;

    [Header("Ataque 3 - Corte Funesto")]
    [SerializeField] private float attack3Cost = 1f;
    [SerializeField] private float cooldown3 = 3f;
    private float attack3Timer = 0f;

    [Header("Ataque 4 - Muerte a la Inversa")]
    [SerializeField] private float attack4Cost = 1f;
    [SerializeField] private float cooldown4 = 4f;
    private float attack4Timer = 0f;

    private void Update()
    {

        attack1Timer = Mathf.Max(attack1Timer - Time.deltaTime, 0f);
        attack2Timer = Mathf.Max(attack2Timer - Time.deltaTime, 0f);
        attack3Timer = Mathf.Max(attack3Timer - Time.deltaTime, 0f);
        attack4Timer = Mathf.Max(attack4Timer - Time.deltaTime, 0f);


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (attack1Timer <= 0f)
            {
                Attack1();
            }
            else
            {
                Debug.Log("Ataque 1 en cooldown.");
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (attack2Timer <= 0f && DolorResourceManager.Instance.MundoActual >= attack2Cost)
            {
                if (DolorResourceManager.Instance.TryConsume(attack2Cost))
                {
                    Attack2();
                    attack2Timer = cooldown2;
                }
            }
            else
            {
                Debug.Log("Ataque 2 en cooldown o sin suficiente recurso");
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (attack3Timer <= 0f && DolorResourceManager.Instance.MundoActual >= attack3Cost)
            {
                if (DolorResourceManager.Instance.TryConsume(attack3Cost))
                {
                    Attack3();
                    attack3Timer = cooldown3;
                }
            }
            else
            {
                Debug.Log("Ataque 3 en cooldown o sin suficiente recurso");
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (attack4Timer <= 0f && DolorResourceManager.Instance.MundoActual >= attack4Cost)
            {
                if (DolorResourceManager.Instance.TryConsume(attack4Cost))
                {
                    Attack4();
                    attack4Timer = cooldown4;
                }
            }
            else
            {
                Debug.Log("Ataque 4 en cooldown o sin suficiente recurso");
            }
        }
    }

    private void Attack1()
    {
        Debug.Log("Ejecutando Ataque 1: Giro Ominoso");

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;

            if (DolorResourceManager.Instance.TryConsume(attack1Cost))
            {
                attack1Timer = cooldown1;
                StartCoroutine(Attack1Movement(targetPos));
            }
            else
            {
                Debug.Log("No hay suficiente energía para Ataque 1.");
            }
        }
        else
        {
            Debug.Log("Posición inválida: el mouse no está sobre el piso (Floor). Ataque cancelado y energía no consumida.");
        }
    }

    private IEnumerator Attack1Movement(Vector3 targetPos)
    {
        if (swordParticleFront != null) swordParticleFront.Play();
        if (swordParticleBack != null) swordParticleBack.Play();

       
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float totalDuration = distance / movementSpeed;
        float spinSpeed = 360f / totalDuration;

        HashSet<Enemy> enemigosDañados = new HashSet<Enemy>();

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

            transform.Rotate(0, spinSpeed * Time.deltaTime, 0, Space.Self);

            Collider[] colisiones = Physics.OverlapSphere(transform.position, collisionRadius, enemyLayer);
            foreach (Collider col in colisiones)
            {
                Enemy enemigo = col.GetComponent<Enemy>();
                if (enemigo != null && !enemigosDañados.Contains(enemigo))
                {
                    enemigo.TakeDamage(attack1Damage);
                    enemigosDañados.Add(enemigo);
                }
            }
            yield return null;
        }

        if (swordParticleFront != null) swordParticleFront.Stop();
        if (swordParticleBack != null) swordParticleBack.Stop();
    }

    private void Attack2()
    {
        Debug.Log("Ejecutando Ataque 2: Danza Fatal");
        // Inserta aquí la lógica específica para el Ataque 2.
    }

    private void Attack3()
    {
        Debug.Log("Ejecutando Ataque 3: Corte Funesto");
        // Inserta aquí la lógica específica para el Ataque 3.
    }

    private void Attack4()
    {
        Debug.Log("Ejecutando Ataque 4: Muerte a la Inversa");
        // Inserta aquí la lógica específica para el Ataque 4.
    }
}
