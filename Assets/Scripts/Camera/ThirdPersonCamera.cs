using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public GameManager gameManager;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -3);
    public float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private Transform model;

    /**
         Metode per a que la MainCamera segueixi la posició del jugador seleccionat.
    **/
    void LateUpdate()
    {
        if (target == null) return;

        model = target.GetChild(gameManager.characterSelected); 

        model.gameObject.SetActive(true);

        if (model == null) return;
        
        Vector3 desiredPosition = model.position + model.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(model.position + Vector3.up * 1.5f);
    }
}
