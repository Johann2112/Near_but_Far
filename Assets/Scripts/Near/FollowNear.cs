using UnityEngine;

public class FollowNear : MonoBehaviour
{
    [SerializeField] private GameObject Near;
    [SerializeField] private float heightOffset = 0.5f;

    private void Update()
    {
        transform.position = new Vector3(Near.transform.position.x, Near.transform.position.y + heightOffset, Near.transform.position.z);
    }
}
