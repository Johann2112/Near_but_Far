using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objetos")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform camTransform;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 6f;
    [SerializeField] private float dashCooldown = 2f;
    private bool canDash = true;

    [HideInInspector] public bool canMove = true;

    private Vector3 moveDirection;
    private float dashTimer;

    private void Update()
    {
        if (!canMove) return;

        HandleMovement();
        HandleDash();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;

            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;

        Vector3 dashDir = transform.forward;
        float dashTime = 0.2f;
        float dashSpeed = dashDistance / dashTime;
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            characterController.Move(dashDir * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
