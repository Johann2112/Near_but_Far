using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float duration = 1.5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }
}
