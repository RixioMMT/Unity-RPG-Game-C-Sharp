using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject interactPanel; // Reference to the Interact Panel GameObject
    public GameObject dialoguePanel; // Reference to the Dialogue Panel GameObject
    public Text dialogueText; // Reference to the Text component in the Dialogue Panel
    public string signMessage = ""; // Message to display
    private bool playerInRange;
    private bool isInteracting;

    void Start()
    {
        interactPanel.SetActive(false); // Hide the interact panel initially
        dialoguePanel.SetActive(false); // Hide the dialogue panel initially
    }

    void Update()
    {
        if (playerInRange && !isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            StartInteraction();
        }
        else if (isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            EndInteraction();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
            interactPanel.SetActive(true); // Show the interact panel
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
            interactPanel.SetActive(false); // Hide the interact panel
            if (isInteracting)
            {
                EndInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        interactPanel.SetActive(false); // Hide the interact panel
        dialoguePanel.SetActive(true); // Show the dialogue panel
        dialogueText.text = signMessage; // Set the dialogue text
    }

    void EndInteraction()
    {
        isInteracting = false;
        dialoguePanel.SetActive(false); // Hide the dialogue panel
        if (playerInRange)
        {
            interactPanel.SetActive(true); // Show the interact panel if player is still in range
        }
    }
}