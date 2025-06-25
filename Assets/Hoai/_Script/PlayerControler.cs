using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerControler : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 2f;
    public float gravity = -9.81f;

    [SerializeField] private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private Animator animator;

    public void SetSpeed(float newSpeed)
    {
        walkSpeed = newSpeed;
    }

    public float GetSpeed()
    {
        return walkSpeed;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Ẩn chuột khi click chuột trái vào màn hình
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Kiểm tra đang chạm đất
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Nhập đầu vào
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hướng di chuyển theo camera
        Vector3 move = Camera.main.transform.right * x + Camera.main.transform.forward * z;
        move.y = 0f;

        bool isMoving = move.magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Xoay nhân vật theo hướng di chuyển
        if (isMoving)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(move),
                Time.deltaTime * 10f
            );
        }

        controller.Move(move.normalized * currentSpeed * Time.deltaTime);

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("nhay"); // Gọi animation nhảy
        }

        //hieu nap dan
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (controller.velocity.magnitude < 0.1f)
            {
                animator.SetTrigger("nap");
                Debug.Log("Đang nạp đạn...");
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

            if (controller.velocity.magnitude < 0.1f)
            {
                animator.SetBool("ngoi", true);
                Debug.Log("ban dang ngoi...");
            }
        }
        if (controller.velocity.magnitude > 0.1f)
        {
            animator.SetBool("ngoi", false);
            Debug.Log("ban dang di chuyen...");
        }

        // nhấn chuột phải để bắn
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("ban");
            Debug.Log("Đang bắn...");
        }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Cập nhật animation
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);
    }
}
