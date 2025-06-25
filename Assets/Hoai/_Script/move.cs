using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 2f;
    public float gravity = -9.81f;
    [SerializeField]

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float GetSpeed()
    {
        return speed;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Mặc định hiện chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Ẩn chuột khi click chuột trái vào màn hình
        if (Input.GetMouseButtonDown(0))
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
        move.y = 0f; // Không cho nhân vật bay lên

        // ✅ Xoay nhân vật theo hướng di chuyển
        if (move.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(move),
                Time.deltaTime * 10f
            );
        }

        controller.Move(move.normalized * speed * Time.deltaTime);

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
