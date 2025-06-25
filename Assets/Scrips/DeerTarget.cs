using UnityEngine;

public class DeerTarget : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullte"))
        {
            // Gọi UI tăng số lượng
            DeerUIManager.Instance.IncreaseDeerCount();

            // Huỷ hươu
            Destroy(gameObject);
        }
    }
}
