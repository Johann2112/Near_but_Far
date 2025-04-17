using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Objetos a buscar")]
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;

    [Header("Valores")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timerToDash;
    [SerializeField] private bool canDash;
    [SerializeField] private float dashDistance;
    private float timer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (input.sqrMagnitude > 0.01f)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;


            Vector3 move = input.normalized * moveSpeed * Time.deltaTime;
            agent.Move(move);

            Quaternion toRotation = Quaternion.LookRotation(input.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime * rotationSpeed);
        }


        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Dash();
        }

        if (!canDash)
        {
            timer += Time.deltaTime;
            if (timer >= timerToDash)
            {
                canDash = true;
                Debug.Log("Can Dash Again");
            }
        }

    }

    private void Dash()
    {
        canDash = false;
        timer = 0;
        StartCoroutine(DashCoroutine());

    }

    private IEnumerator DashCoroutine()
    {
        agent.isStopped = true;
        float dashDuration = 0.3f;
        float elapsedTime = 0f;

        Vector3 dashDirection = transform.forward;
        float dashSpeed = dashDistance / dashDuration;

        while (elapsedTime < dashDuration)
        {
            agent.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        Debug.Log("Finished Dashing");
    }
}
