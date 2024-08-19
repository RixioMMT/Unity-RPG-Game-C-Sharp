using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Camera mainCamera; 
    private NavMeshAgent agent; 

    private SwordAttack swordAttack; 
    private bool isInteracting = false; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        swordAttack = GetComponent<SwordAttack>(); 

        SetInitialRotation();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (swordAttack != null)
        {
            swordAttack.SetInteracting(isInteracting);
        }
    }

    private void SetInitialRotation()
    {
        Quaternion initialRotation = Quaternion.Euler(0, 135, 0);
        transform.rotation = initialRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isInteracting = false;
        }
    }
}