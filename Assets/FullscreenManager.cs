using UnityEngine;
using UnityEngine.UI;

public class FullscreenManager : MonoBehaviour
{
   [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        Fullscreen(Screen.fullScreen);
        fullscreenToggle.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(bool isOn)
    {
        Fullscreen(isOn);
    }

     private void Fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;

        if (fullscreen)
        {
            Debug.Log("Pantalla completa: ACTIVADO");
        }
        else
        {
            Debug.Log("Pantalla completa: DESACTIVADO");
        }
    }
}