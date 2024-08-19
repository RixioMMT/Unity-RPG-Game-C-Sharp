using UnityEngine;
using UnityEngine.UI;

public class AmigurumiNPC : MonoBehaviour
{
    public GameObject player;
    public GameObject interactPanel;
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string initialDialogue = "Vence al enemigo y dejaré que me adoptes. Te daré esta espada.";
    public string receivedItemDialogue = "¡Has recibido una espada azul!";
    public Item blueSword; 

    private bool playerInRange;
    private bool isInteracting;
    private bool givenItem; 
    private InventoryManager inventoryManager;
    private PlayerMovement playerMovement; 

    private void Start()
    {
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(false);

        GameObject inventoryGameObject = GameObject.FindWithTag("GameManager");
        if (inventoryGameObject != null)
        {
            inventoryManager = inventoryGameObject.GetComponent<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager component not found on Player Inventory GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player Inventory GameObject with tag 'GameManager' not found.");
        }

        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on player GameObject.");
        }

        if (blueSword == null)
        {
            Debug.LogError("blueSword is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (playerInRange && !isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            StartInteraction();
        }
        else if (isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            ProgressInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (inventoryManager.HasItem(blueSword))
            {
                Debug.Log("Player already has the blue sword. Skipping interaction.");
                return;
            }

            playerInRange = true;
            interactPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
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

    private void StartInteraction()
    {
        isInteracting = true;
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = initialDialogue;

        if (playerMovement != null)
        {
            playerMovement.enabled = false; 
        }
    }

    private void ProgressInteraction()
    {
        if (!givenItem)
        {
            GiveItem();
        }
        else
        {
            EndInteraction();
        }
    }

    private void GiveItem()
    {
        if (inventoryManager != null && blueSword != null)
        {
            dialogueText.text = receivedItemDialogue;
            inventoryManager.AddItem(blueSword);
            givenItem = true;
        }
        else
        {
            Debug.LogError("InventoryManager or blueSword is null. Cannot complete interaction.");
        }
    }

    private void EndInteraction()
    {
        isInteracting = false;
        playerInRange = false; 
        interactPanel.SetActive(false); 
        dialoguePanel.SetActive(false); 

        if (playerMovement != null)
        {
            playerMovement.enabled = true; 
        }
    }
}