using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameManager gameManager;
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    public static bool isPaused = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();

    }

    /**
         Aplica als enemics un target per a que amb el NavMeshAgent el segueixin, també els atura en altres casos.
    **/
    void Update()
    {

        if (target == null) return;

        if (isPaused || gameManager.isGameOver || gameManager.isWaitingNextLevel || gameManager.isBallKicked)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f, 0f, Time.deltaTime);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(target.position);


        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);

    }
}
