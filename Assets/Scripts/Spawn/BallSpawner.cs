using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform[] ballSpawnsEasy;
    public Transform[] ballSpawnsMedium;
    public Transform[] ballSpawnsHard;

    public Transform ball;

    /**
         Posicionar la pilota segons el nivell i dificultat.
    **/
    public void SpawnBall(GameManager.Difficulty difficulty, int level)
    {
        Transform ballPosition = null;

        switch (difficulty)
        {
            case GameManager.Difficulty.Easy:
                ballPosition = ballSpawnsEasy[level - 1];
                break;
            case GameManager.Difficulty.Medium:
                ballPosition = ballSpawnsMedium[level - 1];
                break;
            case GameManager.Difficulty.Hard:
                ballPosition = ballSpawnsHard[level - 1];
                break;
        }

        Rigidbody ballRB = ball.GetComponentInChildren<Rigidbody>();

        ballRB.linearVelocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        ballRB.position = ballPosition.position;


        Debug.Log($"Spawning Ball at: {ballPosition.position}");

    }
    
}
