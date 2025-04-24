using UnityEngine;

public class DolorSummoning : MonoBehaviour
{
    [Header("Zona permitida")]
    [SerializeField] private float areaRadius;
    [SerializeField] private float areaY;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnHeightOffset;

    [SerializeField] private GameObject prefab2;
    [SerializeField] private float spawnHeightOffset2;

    [Header("Referencias")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem particles;

    [Header("Capa válida")]
    [SerializeField] private LayerMask floorLayerMask;

    private GameObject current;
    private GameObject current2;

    private void Start()
    {
        current2 = Instantiate(prefab2, new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z), Quaternion.identity);
        current2.transform.SetParent(player);
        if (particles != null)
            particles.Play();
    }

    private void Update()
    {
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
                float distanceToCenter = Vector3.Distance(hitPoint, new Vector3(transform.position.x, areaY, transform.position.z));

                if (distanceToCenter <= areaRadius)
                {
                    if (current == null)
                    {
                        if (DolorResourceManager.Instance.IsFull() && !DolorResourceManager.Instance.EnCooldown)
                        {
                            if (current2 != null)
                            {
                                Destroy(current2);
                                current2 = null;
                            }
                            if (particles != null)
                                particles.Stop();
                            Vector3 spawnPos = new Vector3(hitPoint.x, areaY + spawnHeightOffset, hitPoint.z);
                            current = Instantiate(prefab, spawnPos, Quaternion.identity);
                        }
                        else
                        {
                            Debug.Log("Dolor no puede ser invocado. Recurso incompleto o en cooldown.");
                        }
                    }
                    else
                    {
                        if (!DolorResourceManager.Instance.IsFull())
                        {
                            DolorResourceManager.Instance.EntrarEnCooldown();
                        }
                        Destroy(current);
                        current = null;

                        current2 = Instantiate(prefab2, new Vector3(player.position.x, player.position.y + spawnHeightOffset2, player.position.z), Quaternion.identity);
                        current2.transform.SetParent(player);
                        if (particles != null)
                            particles.Play();
                    }

                    if (DolorResourceManager.Instance.MundoActual <= 0)
                    {
                        if (current != null)
                        {
                            Destroy(current);
                            current = null;
                        }
                        DolorResourceManager.Instance.EntrarEnCooldown();
                        Debug.Log("Dolor ha desaparecido por llegar a 0 unidades.");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 areaCenter = new Vector3(transform.position.x, areaY, transform.position.z);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(areaCenter, areaRadius);
    }
}
