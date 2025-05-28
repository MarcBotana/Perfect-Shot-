using UnityEngine;

public class BallGoalChecker : MonoBehaviour
{

    public GameManager gameManager;
    private Rigidbody rb;

    public float stopThreshold = 0.2f;
    public float maxWaitTime = 4f;

    private float waitTimer = 0f;
    private bool waitingForResult = false;
    private bool hasFinished = false;

    private bool hasStartedMoving = false;

    /**
         Obtindre el RigidBody de la pilota.
    **/
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    /**
         Mètode per si la pilota no colisiona ni amb la porteria ni fora del camp.
    **/
    public void StartWaitingForResult()
    {
        waitTimer = 0f;
        waitingForResult = true;
        hasFinished = false;
    }

    /**
         Donar el chut com a invalid si la pilota esta aturada 4 seg.
    **/
    void Update()
    {
        if (!waitingForResult || hasFinished) return;

        waitTimer += Time.deltaTime;

        if (!hasStartedMoving && rb.linearVelocity.magnitude > stopThreshold)
        {
            hasStartedMoving = true;
        }

        if (hasStartedMoving)
        {
            if (waitTimer >= maxWaitTime && rb.linearVelocity.magnitude < stopThreshold)
            {
                hasFinished = true;
                waitingForResult = false;
                gameManager.RegisterFail();
            }
        }
    }

    /**
         Mètode per comprovar la col·lisió de la pilota.
    **/
    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished) return;

        if (other.CompareTag("Goal"))
        {
            hasFinished = true;
            waitingForResult = false;
            gameManager.RegisterGoal();
        }
        else if (other.CompareTag("OutField"))
        {
            hasFinished = true;
            waitingForResult = false;
            gameManager.RegisterFail();
        }
    }




}


