using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    void Start()
    {
        if (toggle != null)
        {
            if (Screen.fullScreen)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
    }
    void Update()
    {

    }

    public void ActiveFullScreen(bool Fullscreen)
    {
        Screen.fullScreen = Fullscreen;
    }
}
