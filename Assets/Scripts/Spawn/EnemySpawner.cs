using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject enemyPrefab;
    public Transform player;
    private Transform model;
    public Transform[] spawnPoints;

    private int enemyCount = 0;

    /**
         Instanciar enemics segons la dificultat.
    **/
    public void SpawnEnemies(GameManager.Difficulty difficulty)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EnemyAI>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        switch (difficulty)
        {
            case GameManager.Difficulty.Easy:
                enemyCount = 3;
                break;
            case GameManager.Difficulty.Medium:
                enemyCount = 5;
                break;
            case GameManager.Difficulty.Hard:
                enemyCount = 7;
                break;
        }

        model = player.GetChild(gameManager.characterSelected);

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, transform);

            Debug.Log($"Spawning Enemies at: {spawnPoint}");

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.gameManager = gameManager;
                ai.target = model;
            }


        }
    }
}

