using UnityEngine;

public class FollowBallPositionOnly : MonoBehaviour
{
    public Transform ball;
    public float heightOffset = 5f;
    public Vector3 fixedRotationEuler = new Vector3(90f, 0f, 0f);

    /**
         Mètode per que la llum de la pilota segueixi la posició de la pilota.
    **/
    void LateUpdate()
    {
        transform.position = ball.position + Vector3.up * heightOffset;
        transform.rotation = Quaternion.Euler(fixedRotationEuler);
    }
}