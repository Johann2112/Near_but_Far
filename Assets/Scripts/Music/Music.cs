using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{

    void Start()
    {
        Invoke("WaitToEnd", 30);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("Menu");
    }


}

