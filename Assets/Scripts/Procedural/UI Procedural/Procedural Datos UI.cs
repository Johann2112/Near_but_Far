using UnityEngine;
using System.Collections.Generic;

public class ProceduralDatosUI : MonoBehaviour
{
    [SerializeField] private ProceduralManager proceduralManager;
    [SerializeField] private int spawnAmountValue;


    private void Start()
    {
     Time.timeScale = 0f;
    }
    public void Change1(bool isCheck)
    {
       if(isCheck)
       {
            spawnAmountValue++;
            Debug.Log("Spawn Amount Increased: " + spawnAmountValue);
            CheckSpawnSize();
        }
       else
       {
            spawnAmountValue--;
            Debug.Log("Spawn Amount Decreased: " + spawnAmountValue);
            CheckSpawnSize();
        }
    }

    public void Change2(bool isCheck)
    {
        if (isCheck)
        {
            spawnAmountValue += 5;
            Debug.Log("Spawn Amount Increased: " + spawnAmountValue);
            CheckSpawnSize();
        }
        else
        {
            spawnAmountValue -= 5;
            Debug.Log("Spawn Amount Decreased: " + spawnAmountValue);
            CheckSpawnSize();
        }
    }

    public void Change3(bool isCheck)
    {
        if (isCheck)
        {
            spawnAmountValue += 10;
            Debug.Log("Spawn Amount Increased: " + spawnAmountValue);
            CheckSpawnSize();
        }
        else
        {
            spawnAmountValue -= 10;
            Debug.Log("Spawn Amount Decreased: " + spawnAmountValue);
            CheckSpawnSize();
        }
    }

    private void CheckSpawnSize()
    {

        if (spawnAmountValue >= 100)
        {
            spawnAmountValue = 100;
        }
        else if (spawnAmountValue <= 0)
        {
            spawnAmountValue = 1;
        }
    }

    public void ApplyChanges()
    {

        proceduralManager.spawnAmounts = spawnAmountValue;
        Time.timeScale = 1f;
    }
}
