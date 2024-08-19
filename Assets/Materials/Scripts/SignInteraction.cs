using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactPanel;
    public GameObject dialoguePanel; 
    public Text dialogueText; 
    public string signMessage = ""; 
    private bool playerInRange;
    private bool isInteracting;

    void Start()
    {
        interactPanel.SetActive(false); 
        dialoguePanel.SetActive(false);
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
            interactPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
            interactPanel.SetActive(false); 
            if (isInteracting)
            {
                EndInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(true); 
        dialogueText.text = signMessage; 
    }

    void EndInteraction()
    {
        isInteracting = false;
        dialoguePanel.SetActive(false); 
        if (playerInRange)
        {
            interactPanel.SetActive(true); 
        }
    }
}