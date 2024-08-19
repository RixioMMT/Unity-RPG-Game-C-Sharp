using UnityEngine;
using UnityEngine.AI;

public class ObstacleController : MonoBehaviour
{
    public NavMeshObstacle rampObstacle;
    public InventoryManager inventoryManager;

    private bool swordEquipped = false; 

    void Update()
    {
        if (inventoryManager != null)
        {
            swordEquipped = inventoryManager.GetEquippedSword() != null;
        }

        rampObstacle.enabled = !swordEquipped;
    }
}