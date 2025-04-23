using UnityEngine;

public class FollowNear : MonoBehaviour
{
    [SerializeField] private GameObject Near;

    private void Update()
    {
        transform.position = new Vector3(Near.transform.position.x, Near.transform.position.y, Near.transform.position.z);
    }
}
