using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public GameManager gameManager;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public static bool isPaused = false;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    
    /**
         Controlar el moviment del personatge amb les tecles (WASD) i correr amb SHIFT.
    **/
    void Update()
    {
        if (isPaused) return;

        if (gameManager.isGameOver || gameManager.isWaitingNextLevel || gameManager.isBallKicked)
        {
            animator.SetFloat("Speed", 0f, 0f, Time.deltaTime);
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y += gravity * 2f * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float inputMagnitude = new Vector2(horizontal, vertical).magnitude;
        float animationSpeed = inputMagnitude * (Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f);
        animator.SetFloat("Speed", animationSpeed, 0.1f, Time.deltaTime);

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);

    }

    /**
         Reescriure la velocitat a 0.
    **/
    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

}
