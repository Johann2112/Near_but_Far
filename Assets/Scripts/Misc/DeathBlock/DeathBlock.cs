using UnityEngine;

public class DeathBlock : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado");
            NearLife nearLife = other.GetComponent<NearLife>();
            if (nearLife != null)
            {
                nearLife.TakeDamage(damage);
                Debug.Log("Se le bajo a Near " + damage + " de vida");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado");
            NearLife nearLife = collision.gameObject.GetComponent<NearLife>();
            if (nearLife != null)
            {
                nearLife.TakeDamage(damage);
                Debug.Log("Se le bajo a Near " + damage + " de vida");
            }
        }
    }
}

