using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;  
    public NavmeshEnemyDetection detectionScript;  
    public Transform hammerPosition;  
    
    private float attackRange = 60f;  
    private float rotationSpeed = 200f; 
    private NavMeshAgent navMeshAgent;  
    private Vector3 originalPosition; 
    private Quaternion originalRotation; 
    private bool isAttacking = false; 
    private Animator hammerAnimator; 

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position; 
        originalRotation = transform.rotation; 

        hammerAnimator = hammerPosition.GetComponentInChildren<Animator>();

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
                navMeshAgent.SetDestination(player.position);

                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            navMeshAgent.SetDestination(originalPosition);

            if (Vector3.Distance(transform.position, originalPosition) < 3f)
            {
                Quaternion targetRotation = originalRotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        return dotProduct > 0.95f; 
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        navMeshAgent.isStopped = true;

        if (hammerAnimator != null)
        {
            hammerAnimator.SetTrigger("Swing");

            yield return new WaitForSeconds(hammerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        navMeshAgent.isStopped = false;
        isAttacking = false;
    }
}