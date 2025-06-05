using UnityEngine;
using UnityEngine.Events;

public class Skipper : MonoBehaviour
{
    [SerializeField] private UnityEvent onSkip; // Evento a invocar al saltar la escena

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            onSkip.Invoke();
        }
    }
}
