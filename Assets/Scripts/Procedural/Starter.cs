using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Starter : MonoBehaviour
{

    [SerializeField] private ProceduralManager proceduralManager;
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    private void Start()
    {
        proceduralManager.Generate();

        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
    }
}
