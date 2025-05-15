using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    private float timeToSpawn = 0;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private int maxEnemiesToSpawn = 5;
    [SerializeField] private int minEnemiesToSpawn = 1;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float offsetY;

    private GameObject pepe;

    private void Update()
    {

        if (spawnOnStart && pepe == null)
        {
            pepe = Instantiate(enemyPrefab);
            pepe.transform.position = gameObject.transform.position;
            pepe.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            pepe.transform.rotation = Quaternion.Euler(35f, 0f, 0f);
            Component[] components = pepe.GetComponents(typeof(Component));
            foreach (Component component in components)
            {
                if (component is Behaviour)
                {
                    Behaviour behaviour = (Behaviour)component;
                    behaviour.enabled = false;
                }

                if (component is Collider)
                {
                    Collider collider = (Collider)component;
                    collider.enabled = false;
                }

                if (component is Boss)
                {
                    Boss boss = (Boss)component;
                    boss.enabled = false;
                }
            }

            spawnOnStart = false;
        }

        if (canSpawn)
        {

            timeToSpawn += Time.deltaTime;

            if (timeToSpawn > spawnInterval)
            {
                SpawnEnemy();
                timeToSpawn = 0;

            }


        }



        if (pepe != null)
        {
            pepe.transform.Rotate(Vector3.up * Time.deltaTime * 100f);
        }

    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawCircle(transform.position, spawnRadius, Vector3.up);
    }

    private void SpawnEnemy()
    {
        int enemyCount = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);

        for (int i = 0; i < enemyCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Mathf.Sqrt(Random.Range(0f,1f) * spawnRadius);
            float x = Mathf.Cos(angle) * distance;
            float z = Mathf.Sin(angle) * distance;
            Vector3 spawnPosition = new Vector3(transform.position.x + x, offsetY, transform.position.z + z);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.position = spawnPosition;


        }
    }
}
