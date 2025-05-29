using System.Collections;
using UnityEngine;

public class NearMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform camTransform;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool canMove = true;
    private Vector3 move;

    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private bool canDash = true;
    [SerializeField] private KeyCode dashKey;

    [Header("Gravedad")]
    [SerializeField] private float gravity;
    private float verticalVelocity;

    public bool CanRotate { get; set; } = true;

    private void Update()
    {
        if (!canMove) return;
        Movement();
        Gravity();
        characterController.Move(move * Time.deltaTime);
        Dash();
    }

    private void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v);
        Vector3 velocity = Vector3.zero;

        if (inputDir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float smoothAngle = transform.eulerAngles.y;
            if (CanRotate)
                smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            velocity = moveDir.normalized * moveSpeed;
        }

        move = new Vector3(velocity.x, verticalVelocity, velocity.z);
    }

    private void Gravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    private void Dash()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
            StartCoroutine(DashC());
    }

    private IEnumerator DashC()
    {
        canDash = false;
        Vector3 dir = transform.forward;
        float speed = dashDistance / dashTime;
        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            characterController.Move(dir * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float DashSpeed
    {
        get { return dashDistance; }
        set { dashDistance = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
}
