using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMonvent : MonoBehaviour
{
    [Header("Cài đặt chuột")]
    public float mouseSensitivity = 100f;      // Độ nhạy chuột

    float xRotation = 0f;                      // Góc xoay theo trục X (lên xuống)
    float yRotation = 0f;                      // Góc xoay theo trục Y (trái phải)

    public float topClamp = -90f;              // Giới hạn nhìn lên
    public float bottomClamp = 90f;            // Giới hạn nhìn xuống

    private bool canLook = true;               // Cho phép điều khiển chuột hay không

    [Header("Tham chiếu đến vũ khí")]
    public Weapon weapon;                      // Để bật/tắt bắn khi khóa chuột

    void Start()
    {
        // Khóa chuột ngay từ đầu khi vào game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Bật bắn nếu có vũ khí
        weapon?.EnableShooting(true);
    }

    void Update()
    {
        // Nhấn Ctrl hoặc ESC để bật/tắt chuột
        if ((Keyboard.current.ctrlKey != null && Keyboard.current.ctrlKey.wasPressedThisFrame)
            || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMouseControl();
        }

        // Nếu không cho phép điều khiển chuột thì không xử lý gì nữa
        if (!canLook) return;

        // Nhận input chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Xử lý xoay trục X (lên xuống) và Y (trái phải)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);  // Giới hạn lên/xuống
        yRotation += mouseX;

        // Gán góc quay vào Transform nhân vật
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    // Hàm bật/tắt điều khiển chuột & bắn
    void ToggleMouseControl()
    {
        canLook = !canLook;

        if (canLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            weapon?.EnableShooting(true); // Bật bắn lại
            Debug.Log("Bật điều khiển chuột & bắn");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            weapon?.EnableShooting(false); // Tắt bắn để tránh bắn nhầm
            Debug.Log("Tắt điều khiển chuột & bắn");
        }
    }
}
