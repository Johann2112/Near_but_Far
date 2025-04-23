using UnityEngine;

public class SpawnerDolor : MonoBehaviour
{
    [Header("Zona permitida")]
    public float areaRadius = 5f;
    public float areaY = 0f; // Altura plana del área donde se puede instanciar

    [Header("Prefab a instanciar")]
    public GameObject prefab;
    public float spawnHeightOffset = 0.1f;

    [Header("Referencias")]
    public Transform player;
    public Camera mainCamera;

    private GameObject currentInstance;

    void Update()
    {
        // El centro del área sigue al jugador
        Vector3 areaCenter = new Vector3(player.position.x, areaY, player.position.z);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, areaY, 0));

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                float distToCenter = Vector3.Distance(hitPoint, areaCenter);

                if (distToCenter <= areaRadius)
                {
                    if (currentInstance == null)
                    {
                        Vector3 spawnPos = new Vector3(hitPoint.x, areaY + spawnHeightOffset, hitPoint.z);
                        currentInstance = Instantiate(prefab, spawnPos, Quaternion.identity);
                    }
                    else
                    {
                        Destroy(currentInstance);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 areaCenter = new Vector3(player.position.x, areaY, player.position.z);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(areaCenter, areaRadius);
        }
    }
}
