using UnityEngine;

public class TargetRespawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void OnHit()
    {
        if (rb != null)
        {
            // Dừng vật lý
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Ngừng chịu lực
        }

        // Ẩn object
        gameObject.SetActive(false);

        // Hồi sinh sau 3 giây
        Invoke(nameof(Respawn), 3f);
    }

    private void Respawn()
    {
        // Đưa về chỗ cũ
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Hiện lại
        gameObject.SetActive(true);

        if (rb != null)
        {
            // Bật lại vật lý sau khi hiện
            rb.isKinematic = false;
        }
    }
}
