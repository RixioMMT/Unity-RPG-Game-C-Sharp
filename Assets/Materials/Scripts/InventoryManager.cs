using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject player;
    public GameObject inventoryUI;
    public Text titleText;
    public Image[] itemSlots;
    public Text[] itemSlotNames;
    public Text itemDescriptionText;
    public Text unequipText;
    public Text cancelText;

    public Item[] items; // Array of items in the inventory
    public Sprite defaultSprite;

    public Transform swordHolder;

    private bool isInventoryOpen = false;
    public bool IsInventoryOpen => isInventoryOpen;

    private int selectedItemIndex = 0;
    private int totalSlots;

    private const int unequipIndex = -1;
    private const int cancelIndex = -2;

    private GameObject equippedItem;
    private bool justOpenedInventory = false; // Flag to indicate if inventory was just opened

    private SwordAttack swordAttack;

    private void Start()
    {
        inventoryUI.SetActive(false);
        titleText.text = "Player Inventory";
        totalSlots = itemSlots.Length;

        if (items.Length != totalSlots)
        {
            items = new Item[totalSlots]; // Initialize items array to match slots
        }

        Debug.Log($"Inventory initialized with {items.Length} slots.");

        UpdateInventoryUI();

        // Find the SwordAttack script on the player
        swordAttack = player.GetComponent<SwordAttack>();
        if (swordAttack == null)
        {
            Debug.LogError("SwordAttack script not found on the player.");
        }
    }

    private void Update()
    {
        if (isInventoryOpen)
        {
            if (justOpenedInventory)
            {
                justOpenedInventory = false; // Reset the flag after the first frame
            }
            else
            {
                HandleInventoryInput();
            }
        }
    }

    private void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedItemIndex >= 0 && selectedItemIndex < totalSlots)
            {
                EquipItem(selectedItemIndex);
            }
            else if (selectedItemIndex == unequipIndex)
            {
                UnequipItem();
            }
            else if (selectedItemIndex == cancelIndex)
            {
                CloseInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(1);
        }
    }

    private void MoveSelection(int direction)
    {
        int newIndex = selectedItemIndex;

        if (selectedItemIndex >= 0 && selectedItemIndex < totalSlots)
        {
            if (direction == 1)
            {
                newIndex++;
                if (newIndex >= totalSlots)
                    newIndex = unequipIndex;
            }
            else if (direction == -1)
            {
                newIndex--;
                if (newIndex < 0)
                    newIndex = cancelIndex;
            }
        }
        else if (selectedItemIndex == unequipIndex)
        {
            if (direction == 1)
            {
                newIndex = cancelIndex;
            }
            else if (direction == -1)
            {
                newIndex = totalSlots - 1;
            }
        }
        else if (selectedItemIndex == cancelIndex)
        {
            if (direction == 1)
            {
                newIndex = 0;
            }
            else if (direction == -1)
            {
                newIndex = unequipIndex;
            }
        }

        if (newIndex >= -2 && newIndex < totalSlots)
        {
            selectedItemIndex = newIndex;
            UpdateSelectedItem();
        }
    }

    private void UpdateSelectedItem()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].color = Color.white;
        }
        unequipText.color = Color.black;
        cancelText.color = Color.black;

        if (selectedItemIndex >= 0 && selectedItemIndex < totalSlots)
        {
            itemSlots[selectedItemIndex].color = Color.yellow;

            if (selectedItemIndex < items.Length && items[selectedItemIndex] != null)
            {
                itemDescriptionText.text = items[selectedItemIndex].itemDescription;
                itemSlotNames[selectedItemIndex].text = items[selectedItemIndex].itemName;
                itemSlots[selectedItemIndex].sprite = items[selectedItemIndex].itemImage;
            }
            else
            {
                itemDescriptionText.text = "";
                itemSlotNames[selectedItemIndex].text = "";
                itemSlots[selectedItemIndex].sprite = defaultSprite;
            }
        }
        else if (selectedItemIndex == unequipIndex)
        {
            unequipText.color = Color.red;
            itemDescriptionText.text = "";
        }
        else if (selectedItemIndex == cancelIndex)
        {
            cancelText.color = Color.red;
            itemDescriptionText.text = "";
        }
    }

    public void EquipItem(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            Item itemToEquip = items[index];
            
            if (itemToEquip == null)
            {
                Debug.LogWarning($"Item at index {index} is null.");
                return;
            }

            if (itemToEquip.isSword)
            {
                Debug.Log($"Equip sword item at index {index}");

                if (equippedItem != null)
                {
                    Destroy(equippedItem); // Remove previous sword
                }

                if (itemToEquip.itemPrefab != null)
                {
                    equippedItem = Instantiate(itemToEquip.itemPrefab, swordHolder.position, swordHolder.rotation, swordHolder);
                    equippedItem.SetActive(true); // Make sure sword is active
                    Debug.Log("Sword equipped at position: " + swordHolder.position);

                    // Notify SwordAttack about the equipped sword and its name
                    if (swordAttack != null)
                    {
                        swordAttack.SetEquippedSword(equippedItem, itemToEquip.itemName);
                    }
                    else
                    {
                        Debug.LogError("SwordAttack script not found on the player.");
                    }
                }
                else
                {
                    Debug.LogWarning($"itemPrefab for item {itemToEquip.itemName} is not assigned.");
                }
            }
            else
            {
                Debug.Log($"Item at index {index} is not a sword.");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid index {index}. Index must be within range of the items array.");
        }
    }

    public void UnequipItem()
    {
        Debug.Log("Unequip item");

        if (equippedItem != null)
        {
            Destroy(equippedItem); // Remove sword
            equippedItem = null;

            // Notify SwordAttack that the sword is unequipped
            if (swordAttack != null)
            {
                swordAttack.SetEquippedSword(null, null);
            }
            else
            {
                Debug.LogError("SwordAttack script not found on the player.");
            }
        }
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        selectedItemIndex = 0;
        inventoryUI.SetActive(true);
        UpdateInventoryUI();
        UpdateSelectedItem();
        DisablePlayerMovement();
        justOpenedInventory = true; // Set the flag when the inventory is opened
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventoryUI.SetActive(false);
        EnablePlayerMovement();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Length && items[i] != null)
            {
                itemSlots[i].sprite = items[i].itemImage;
                itemSlotNames[i].text = items[i].itemName;
            }
            else
            {
                itemSlots[i].sprite = defaultSprite;
                itemSlotNames[i].text = "";
            }
        }

        unequipText.gameObject.SetActive(true);
        cancelText.gameObject.SetActive(true);
    }

    private void DisablePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void EnablePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log($"Added item {item.itemName} to inventory at slot {i}.");
                UpdateInventoryUI();
                break;
            }
        }
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public GameObject GetEquippedSword()
    {
        return equippedItem;
    }
}