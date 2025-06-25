using System.Collections;
using UnityEngine;

public class ScopeEffectHandler : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera; // Camera của người chơi, được dùng để làm hiệu ứng ngắm

    [Header("Breathing Effect")]
    public float breathIntensity = 0.05f; // Độ mạnh của hiệu ứng thở (dao động trục Y)
    public float breathSpeed = 1.5f;      // Tốc độ dao động

    [Header("Recoil Effect")]
    public float recoilAmount = 0.05f;    // Độ giật (dịch camera Y+ và Z-)
    public float recoilSpeed = 10f;       // Tốc độ giật và hồi lại

    private Vector3 originalLocalPos;     // Vị trí gốc của camera (dùng để hoàn trả sau hiệu ứng)
    private Coroutine recoilCoroutine;    // Coroutine hiện tại đang thực thi hiệu ứng giật

    private void Awake()
    {
        if (playerCamera != null)
            originalLocalPos = playerCamera.transform.localPosition; // Lưu vị trí ban đầu
    }

    /// <summary>
    /// Gọi trong Update để áp dụng hiệu ứng thở khi đang ngắm
    /// </summary>
    /// <param name="isScoped">Có đang scope không?</param>
    public void ApplyBreathing(bool isScoped)
    {
        if (playerCamera == null) return;

        if (isScoped)
        {
            // Tính toán offset lên/xuống bằng sin(time)
            float offsetY = Mathf.Sin(Time.time * breathSpeed) * breathIntensity;

            // Đặt vị trí camera với offset Y (vẫn giữ X và Z ban đầu)
            playerCamera.transform.localPosition = originalLocalPos + new Vector3(0, offsetY, 0);
        }
        else
        {
            // Nếu không scope, trả lại vị trí ban đầu một cách mượt
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                originalLocalPos,
                Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// Gọi khi bắn để bắt đầu hiệu ứng giật camera
    /// </summary>
    public void TriggerRecoil()
    {
        // Nếu đang giật mà bắn tiếp, thì dừng coroutine cũ lại
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        // Bắt đầu hiệu ứng giật mới
        recoilCoroutine = StartCoroutine(DoRecoil());
    }

    /// <summary>
    /// Coroutine xử lý hiệu ứng giật camera về phía sau và trả lại vị trí ban đầu
    /// </summary>
    private IEnumerator DoRecoil()
    {
        if (playerCamera == null) yield break;

        // Tạo vị trí giật lùi (Z- để lùi về sau, Y+ để hơi nâng lên)
        Vector3 recoilBack = originalLocalPos + new Vector3(0, recoilAmount, -recoilAmount);
        float t = 0;

        // Pha 1: giật lùi từ vị trí ban đầu đến vị trí giật
        while (t < 1)
        {
            t += Time.deltaTime * recoilSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(originalLocalPos, recoilBack, t);
            yield return null;
        }

        // Pha 2: trả ngược từ vị trí giật về vị trí gốc
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * recoilSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(recoilBack, originalLocalPos, t);
            yield return null;
        }
    }

    /// <summary>
    /// Reset lại vị trí gốc camera, dùng khi tắt scope hoặc thay vũ khí
    /// </summary>
    public void ResetPosition()
    {
        if (playerCamera != null)
            playerCamera.transform.localPosition = originalLocalPos;
    }
}
