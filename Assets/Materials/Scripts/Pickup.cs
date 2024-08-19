using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public GameObject warningPanel;

    private bool playerInRange;

    private void Start()
    {
        warningPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            warningPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            warningPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(item);
                Destroy(gameObject);
                warningPanel.SetActive(false);
            }
        }
    }
}