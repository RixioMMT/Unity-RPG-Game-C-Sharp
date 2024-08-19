using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public NavmeshEnemyDetection detectionScript;  // Reference to the NavmeshEnemyDetection script
    public Transform hammerPosition;  // Reference to the transform where the hammer should be equipped
    
    private float attackRange = 60f;  // The range within which the enemy will attack
    private float rotationSpeed = 200f;  // Speed at which the enemy rotates towards the player
    private NavMeshAgent navMeshAgent;  // Reference to the NavMeshAgent component
    private Vector3 originalPosition;  // The enemy's original position
    private Quaternion originalRotation;  // The enemy's original rotation
    private bool isAttacking = false;  // To check if the enemy is currently attacking
    private Animator hammerAnimator;  // Reference to the hammer's Animator component

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;  // Store the original position of the enemy
        originalRotation = transform.rotation;  // Store the original rotation of the enemy

        // Find the hammer's Animator component in the child GameObject
        hammerAnimator = hammerPosition.GetComponentInChildren<Animator>();

        // Ensure the hammer is initially inactive
        if (hammerAnimator != null)
        {
            hammerAnimator.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isAttacking) return;

        if (detectionScript.playerInArea)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && IsFacingPlayer())
            {
                StartCoroutine(Attack());
            }
            else
            {
                // Follow the player
                navMeshAgent.SetDestination(player.position);

                // Smoothly rotate the enemy towards the player
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Return to the original position
            navMeshAgent.SetDestination(originalPosition);

            // Check if the enemy is close to the original position
            if (Vector3.Distance(transform.position, originalPosition) < 3f)
            {
                // Smoothly rotate back to the original rotation
                Quaternion targetRotation = originalRotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        return dotProduct > 0.95f;  // Adjust this threshold if needed (near 1 means the direction is close to forward)
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        // Stop the enemy from moving
        navMeshAgent.isStopped = true;

        // Trigger the hammer's attack animation
        if (hammerAnimator != null)
        {
            hammerAnimator.SetTrigger("Swing");

            // Wait for the animation to finish
            yield return new WaitForSeconds(hammerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        // Resume movement after attack
        navMeshAgent.isStopped = false;
        isAttacking = false;
    }
}