using UnityEngine;

public class DolorSummoning : MonoBehaviour
{
    [Header("Zona permitida")]
    [SerializeField] private float areaRadius;
    [SerializeField] private float areaY;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnHeightOffset;

    [SerializeField] private GameObject prefab2; // Dolor en estado chill.
    [SerializeField] private float spawnHeightOffset2; // Pos de Dolor en estado chill.

    [Header("Referencias")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player; // Referencia al Player.
    [SerializeField] private ParticleSystem particles; // Referencia a las particulas.

    [Header("Capa válida")]
    [SerializeField] private LayerMask floorLayerMask;

    private GameObject current; // Dolor en estado no chill.
    private GameObject current2; // Dolor en estado chill.

    private void Start()
    {
        current2 = Instantiate(prefab2, new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z), Quaternion.identity);
        current2.transform.SetParent(player);
        particles.Play();
    }

    private void Update()
    {
        Vector3 area = new Vector3(transform.position.x, areaY, transform.position.z);

        if (current2 != null)
        {
            current2.transform.position = new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 100f, floorLayerMask))
            {
                Vector3 hitPoint = hitinfo.point;
                float disToCenter = Vector3.Distance(hitPoint, new Vector3(transform.position.x, areaY, transform.position.z));


                if (disToCenter <= areaRadius)
                {
                    if (current == null)
                    {
                        Destroy(current2);

                        particles.Stop();
                        Vector3 spawnPos = new Vector3(hitPoint.x, areaY + spawnHeightOffset, hitPoint.z);
                        current = Instantiate(prefab, spawnPos, Quaternion.identity);
                    }
                    else
                    {
                        particles.Play();
                        Destroy(current);
                        current2 = Instantiate(prefab2, new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z), Quaternion.identity);
                        current2.transform.SetParent(player);
                    }
                }
                else
                {
                    if (current != null)
                    {
                        particles.Play();
                        Destroy(current);
                        current2 = Instantiate(prefab2, new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z), Quaternion.identity);
                        current2.transform.SetParent(player);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Vector3 areaCenter = new Vector3(transform.position.x, areaY, transform.position.z);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(areaCenter, areaRadius);
    }
}