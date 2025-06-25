using UnityEngine;

public class PlayerMonvent : MonoBehaviour
{
    private CharacterController controller;

    [Header("Di chuyển")]
    public float walkSpeed = 6f;            // Tốc độ đi bộ
    public float sprintSpeed = 12f;         // Tốc độ khi chạy (nhấn Shift)

    [Header("Nhảy")]
    public float jumpHeight = 3f;           // Độ cao khi nhảy
    public int maxJumps = 2;                // Số lần nhảy tối đa (hỗ trợ nhảy đôi)

    [Header("Trọng lực")]
    public float gravity = -9.18f * 2;      // Trọng lực được nhân đôi để rơi nhanh hơn

    [Header("Kiểm tra mặt đất")]
    public Transform groundCheck;           // Điểm kiểm tra va chạm mặt đất
    public float groundDistance = 0.4f;     // Bán kính kiểm tra
    public LayerMask groundMask;            // Lớp mặt đất

    private Vector3 velocity;               // Vector vận tốc theo trục Y
    private int jumpCount = 0;              // Số lần đã nhảy
    private bool isGrounded;                // Đang đứng trên mặt đất?

    [Header("Vũ khí")]
    public Weapon weapon;                   // Tham chiếu tới vũ khí để điều khiển animation

    // Ghi nhớ trạng thái đã từng chạy (khi còn trên mặt đất)
    private bool wasRunning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Kiểm tra xem đang đứng trên mặt đất hay không
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;      // Reset vận tốc rơi khi chạm đất
            jumpCount = 0;         // Reset số lần nhảy
        }

        // Nhận input di chuyển
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift); // Nhấn Shift để chạy

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Di chuyển theo hướng input
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Nhảy nếu nhấn phím "Jump" và chưa vượt quá số lần nhảy tối đa
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Tính vận tốc nhảy
            jumpCount++;
            SoundManagar.Instance?.PlayJump(); // Phát âm thanh nhảy
        }

        // Áp dụng trọng lực dần theo thời gian
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // Di chuyển theo trục Y

        // Xử lý âm thanh chạy hoặc đi bộ
        bool isMoving = move.magnitude > 0.1f;
        if (isGrounded && isMoving)
        {
            if (isSprinting)
            {
                SoundManagar.Instance?.PlayRun();   // Âm thanh chạy
                SoundManagar.Instance?.StopMove();  // Dừng âm thanh đi bộ
            }
            else
            {
                SoundManagar.Instance?.PlayMove();  // Âm thanh đi bộ
                SoundManagar.Instance?.StopRun();   // Dừng âm thanh chạy
            }
        }
        else
        {
            // Không di chuyển → dừng cả hai âm thanh
            SoundManagar.Instance?.StopMove();
            SoundManagar.Instance?.StopRun();
        }

        // Gửi trạng thái chạy đến Animator vũ khí
        if (weapon != null)
        {
            // Đang chạy bình thường?
            bool isRunningNow = isGrounded && isMoving && isSprinting;

            // Nếu đang chạy thì lưu trạng thái
            if (isRunningNow)
                wasRunning = true;

            // Nếu đang ở trên không mà trước đó có chạy → giữ animation chạy
            bool shouldSetRunning = isRunningNow || (!isGrounded && wasRunning);

            // Nếu nhảy mà không chạy trước đó → không giữ animation chạy
            if (!isGrounded && !wasRunning)
                shouldSetRunning = false;

            // Gửi trạng thái tới Animator vũ khí
            weapon.SetRunningState(shouldSetRunning);

            // Nếu đã chạm đất và không còn chạy → reset trạng thái
            if (isGrounded && !isRunningNow)
                wasRunning = false;
        }
    }
}
