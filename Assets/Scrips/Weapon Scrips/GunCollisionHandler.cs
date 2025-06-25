using UnityEngine;

public class GunCollisionHandler : MonoBehaviour
{
    public Transform gunTransform; // Gán khẩu súng vào đây
    private Vector3 originalLocalPos;

    private void Start()
    {
        if (gunTransform == null)
            gunTransform = transform.parent; // auto lấy cha nếu chưa gán

        originalLocalPos = gunTransform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            // Rút súng vào để không xuyên
            gunTransform.localPosition = new Vector3(0f, 0f, 3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            // Trả súng về vị trí ban đầu
            gunTransform.localPosition = originalLocalPos;
        }
    }
}