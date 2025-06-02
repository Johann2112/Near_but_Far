using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProceduralManager : MonoBehaviour
{
    [Header("Objetos A - Zona Segura")]
    [SerializeField] private List<GameObject> platSafe = new List<GameObject>();

    [Header ("Objetos B - Zona Neutral")]
    [SerializeField] private List<GameObject> platNeutral = new List<GameObject>();

    [Header("Objetos C - Zona Peligrosa")]
    [SerializeField] private List<GameObject> platDanger = new List<GameObject>();

    [Header("Configuración de Spawn")]
    [SerializeField] private int spawnAmount;
    [SerializeField] private int spawnedAmount;
    [SerializeField] private int amountToChangeOffset;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 spawnOffsetChange;


    [SerializeField] private Vector3 spawnOffsetLine1; //primer cambio de offet
    [SerializeField] private Vector3 spawnOffsetLine2; //segundo cambio de offset
    [SerializeField] private Vector3 spawnOffsetLine3;  //tercer cambio de offset
    [SerializeField] private Vector3 spawnOffsetLine4; //cuarto cambio de offset
    [SerializeField] private int amountLine1; // cantidad de objetos en la primera línea antes de cambiar el offset
    [SerializeField] private int amountLine2; // cantidad de objetos en la segunda línea antes de cambiar el offset
    [SerializeField] private int amountLine3; // cantidad de objetos en la tercera línea antes de cambiar el offset


    private void Start()
    {



        foreach (GameObject plat in platSafe)
        {
            Debug.Log("Zona Segura: " + plat.name);
        }

        foreach (GameObject plat in platNeutral)
        {
            Debug.Log("Zona Neutral: " + plat.name);
        }

        foreach (GameObject plat in platDanger)
        {
            Debug.Log("Zona Peligrosa: " + plat.name);
        }



        Generate();
    }


    private void Generate()
    {

        int previousType = 0; //0 = zonas seguras 1 = zonas neutras 2 = zonas peligrosas 
        Vector3 currentPos = transform.position;

        for (int i = 0; i < spawnAmount; i++)
        {
           
            List<int> typesAllowed; //para respetar las reglas de placement
            typesAllowed = new List<int>();

            switch (previousType)
            {
                case 0: //zona segura
                    typesAllowed.Add(0); //zona segura
                    typesAllowed.Add(1); //zona neutral
                    break;
                case 1: //zona neutral
                    typesAllowed.Add(0); //zona segura
                    typesAllowed.Add(1); //zona neutral
                    typesAllowed.Add(2); //zona peligrosa
                    break;
                case 2: //zona peligrosa
                    typesAllowed.Add(1); //zona neutral
                    typesAllowed.Add(2); //zona peligrosa
                    break;
                default:
                    Debug.LogError("Tipo de zona no reconocido");
                    return;
            }

            int selectedType = typesAllowed[Random.Range(0, typesAllowed.Count)];
            previousType = selectedType;

            GameObject spawnPrefab = null;

            if (selectedType == 0 && platSafe.Count  > 0)
            {
                spawnPrefab = platSafe[Random.Range(0, platSafe.Count)];
            }
            else if (selectedType == 1 && platNeutral.Count > 0)
            {
                spawnPrefab = platNeutral[Random.Range(0, platNeutral.Count)];
            }
            else if (selectedType == 2 && platDanger.Count > 0)
            {
                spawnPrefab = platDanger[Random.Range(0, platDanger.Count)];
            }

            if (spawnPrefab != null )
            {
                Instantiate(spawnPrefab, currentPos, Quaternion.identity);
                spawnedAmount++;

                if (spawnedAmount < amountLine1)
                {
                    currentPos += spawnOffsetLine1;
                }
                else if (spawnedAmount < amountLine2)
                {
                    
                    currentPos += spawnOffsetLine2;
                }
                else if (spawnedAmount < amountLine3)
                {
                    currentPos += spawnOffsetLine3;
                }
                else
                {
                    currentPos += spawnOffsetLine4;
                }
            }
        }


    
    }
}
