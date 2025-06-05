using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> fixedRotationObjects = new List<GameObject>();
    [SerializeField] private int minAmount = 0;
    [SerializeField] private int maxAmount = 0;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private float offsetY = 1f;

    private void Start()
    {
        if (spawnableObjects.Count == 0 && fixedRotationObjects.Count == 0)
        {
            Debug.LogWarning("No objects to spawn");
            return;
        }

        if (spawnArea == null)
        {
            Debug.LogWarning("Spawn area not set");
            return;
        }

        int amountToSpawn = Random.Range(minAmount, maxAmount + 1);
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject objectToSpawn;
            Quaternion rotation;

            bool useFixedRotationList = Random.value > 0.5f && fixedRotationObjects.Count > 0;

            if (useFixedRotationList)
            {
                int randomIndex = Random.Range(0, fixedRotationObjects.Count);
                objectToSpawn = fixedRotationObjects[randomIndex];
                rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            }
            else
            {
                int randomIndex = Random.Range(0, spawnableObjects.Count);
                objectToSpawn = spawnableObjects[randomIndex];
                rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            }

            Vector3 boundsMin = spawnArea.bounds.min;
            Vector3 boundsMax = spawnArea.bounds.max;

            float randomX = Random.Range(boundsMin.x, boundsMax.x);
            float randomZ = Random.Range(boundsMin.z, boundsMax.z);

            Vector3 randomPos = new Vector3(randomX, transform.position.y + offsetY, randomZ);

            Instantiate(objectToSpawn, randomPos, rotation);
        }
    }
}
