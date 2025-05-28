using UnityEngine;

public class KickTrigger : MonoBehaviour
{
    public Camera kickCamera;
    public Transform ball;
    public float distanceFromBall = 2f;
    public float heightOffset = 1f;
    public GameManager gameManager;

    /**
         Mètode per posicionar la KickCamera (camera de la pilota) desde la direcció en que ha tocat el Player.
    **/
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameManager.isBallKicked) return;

        Vector3 direction = (ball.position - other.transform.position);
        direction.y = 0f;
        direction.Normalize();

        Vector3 cameraPosition = ball.position - direction * distanceFromBall;
        cameraPosition.y = ball.position.y + heightOffset;

        kickCamera.transform.position = cameraPosition;

        Vector3 lookTarget = ball.position + direction * 10f;
        lookTarget.y = kickCamera.transform.position.y;
        kickCamera.transform.LookAt(lookTarget);

        gameManager.EnterKickMode();
    }
}

