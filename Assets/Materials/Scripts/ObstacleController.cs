using UnityEngine;
using UnityEngine.AI;

public class ObstacleController : MonoBehaviour
{
    public NavMeshObstacle rampObstacle;
    public InventoryManager inventoryManager; // Reference to the InventoryManager

    private bool swordEquipped = false; // Track if a sword is equipped

    void Update()
    {
        // Check if a sword is equipped using the InventoryManager
        if (inventoryManager != null)
        {
            swordEquipped = inventoryManager.GetEquippedSword() != null;
        }

        // Enable or disable the obstacle based on swordEquipped
        rampObstacle.enabled = !swordEquipped;
    }
}