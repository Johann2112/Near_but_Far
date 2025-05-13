using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class NearMovement : MonoBehaviour
{

    #region Referencias
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform camTransform;
    #endregion

    #region Variables
    #region Variables de movimiento
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool canMove;
    private Vector3 move;
    #endregion

    #region Variables de dash
    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashElapsedTime;
    [SerializeField] private bool canDash = true;
    [SerializeField] private KeyCode dashKey;
    #endregion

    #region Variables de gravedad
    [Header("Gravedad")]
    [SerializeField] private float gravity;
    private float verticalVelocity;
    #endregion
    #endregion

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        Movement();
        Gravity();
        characterController.Move(move * Time.deltaTime);

        Dash();
    }

    #region Funcionamiento de Movimiento
    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        Vector3 rep = Vector3.zero;

        if (inputDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moverDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rep = moverDir.normalized * moveSpeed;
        }

        move = new Vector3(rep.x, verticalVelocity, rep.z);


    }
    #endregion

    #region Funcionamiento del Dash
    private void Dash()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(DashC());
        }
        else
        {
            Debug.Log("Dash cannot be used right now");
        }

    }

    private IEnumerator DashC()
    {
        canDash = false;

        Vector3 dashDir = transform.forward;
        float dashSpeed = dashDistance / dashTime;
        dashElapsedTime = 0f;

        while (dashElapsedTime < dashTime)
        {
            characterController.Move(dashDir * dashSpeed * Time.deltaTime);
            dashElapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion

    private void Gravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {

            verticalVelocity += gravity * Time.deltaTime;
        }

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
