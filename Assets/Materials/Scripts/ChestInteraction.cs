using UnityEngine;
using UnityEngine.UI;

public class ChestInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactPanel;
    public GameObject chestPanel;
    public Text chestText;
    public Item itemInChest;
    public bool enemyDefeated = false; // Boolean to check if the enemy is defeated

    private bool playerInRange;
    private bool isInteracting;
    private InventoryManager inventoryManager; // Reference to the InventoryManager

    void Start()
    {
        interactPanel.SetActive(false);
        chestPanel.SetActive(false);

        // Find the InventoryManager using the GameObject tagged "GameManager"
        GameObject inventoryGameObject = GameObject.FindWithTag("GameManager");
        if (inventoryGameObject != null)
        {
            inventoryManager = inventoryGameObject.GetComponent<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager component not found on GameManager GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameManager GameObject with tag 'GameManager' not found.");
        }
    }

    void Update()
    {
        if (playerInRange && !isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            if (enemyDefeated)
            {
                StartInteraction();
            }
            else
            {
                chestText.text = "Vence el enemigo para poder abrir el cofre.";
                chestPanel.SetActive(true);
            }
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
            chestPanel.SetActive(false); // Ensure chestPanel is hidden when exiting range
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

        // Check if the item is already in the inventory
        if (inventoryManager != null && inventoryManager.HasItem(itemInChest))
        {
            chestText.text = "Ya tienes una \"" + itemInChest.itemName + "\".";
        }
        else
        {
            // Show item name in quotes
            chestText.text = "¡Encontraste una \"" + itemInChest.itemName + "\"!";

            // Add item to the inventory
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemInChest);
            }
            else
            {
                Debug.LogWarning("No InventoryManager found. Item not added to inventory.");
            }
        }

        chestPanel.SetActive(true);
    }

    void EndInteraction()
    {
        isInteracting = false;
        chestPanel.SetActive(false);
        if (playerInRange)
        {
            interactPanel.SetActive(true);
        }
    }
}