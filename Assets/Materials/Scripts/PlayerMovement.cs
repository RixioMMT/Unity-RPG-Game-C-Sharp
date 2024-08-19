using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public Camera mainCamera; // Reference to the main camera
    private NavMeshAgent agent; // Reference to the NavMeshAgent

    private SwordAttack swordAttack; // Reference to the SwordAttack script
    private bool isInteracting = false; // Flag to check interaction status

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        swordAttack = GetComponent<SwordAttack>(); // Assuming SwordAttack is on the same GameObject

        // Set the initial rotation to face the Z axis
        SetInitialRotation();
    }

    private void Update()
    {
        // Get input from the arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Flatten the vectors to the horizontal plane (ignore y component)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction relative to the camera
        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Update sword interaction status
        if (swordAttack != null)
        {
            swordAttack.SetInteracting(isInteracting);
        }
    }

    private void SetInitialRotation()
    {
        // Set the player's rotation to face the Z axis
        Quaternion initialRotation = Quaternion.Euler(0, 135, 0); // Modify if necessary
        transform.rotation = initialRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has a tag that should prevent sword swinging
        if (other.CompareTag("Interactable"))
        {
            isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider has a tag that should prevent sword swinging
        if (other.CompareTag("Interactable"))
        {
            isInteracting = false;
        }
    }
}