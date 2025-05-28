using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public Transform easySpawnLeft;
    public Transform easySpawnRight;

    public Transform mediumSpawnLeft;
    public Transform mediumSpawnRight;

    public Transform hardSpawnLeft;
    public Transform hardSpawnRight;

    public Transform player;

    private Transform model;

    /**
         Posicionar Player segons dificultat en un punt aleatori.
    **/
    public void SpawnPlayer(GameManager.Difficulty difficulty)
    {
        Transform left = null, right = null;

        model = player.GetChild(gameManager.characterSelected);

        ThirdPersonController controller = model.GetComponent<ThirdPersonController>();
        CharacterController cc = model.GetComponent<CharacterController>();


        switch (difficulty)
        {
            case GameManager.Difficulty.Easy:
                left = easySpawnLeft;
                right = easySpawnRight;
                break;
            case GameManager.Difficulty.Medium:
                left = mediumSpawnLeft;
                right = mediumSpawnRight;
                break;
            case GameManager.Difficulty.Hard:
                left = hardSpawnLeft;
                right = hardSpawnRight;
                break;
        }

        controller.ResetVelocity();

        float t = Random.Range(0f, 1f);
        Vector3 spawnPosition = Vector3.Lerp(left.position, right.position, t);

        cc.enabled = false;

        model.position = spawnPosition;
        model.rotation = Quaternion.LookRotation(Vector3.right);

        cc.enabled = true;

        Debug.Log($"Spawning Player at: {spawnPosition}");

    }
}
