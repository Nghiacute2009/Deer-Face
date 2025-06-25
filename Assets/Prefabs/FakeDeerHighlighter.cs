using UnityEngine;
using System.Collections;

public class FakeDeerHighlighter : MonoBehaviour
{
    public Renderer deerRenderer; // Renderer của nai giả (MeshRenderer hoặc SkinnedMeshRenderer)
    public Color flashColor = Color.red; // Màu khi nhá đỏ
    public float flashDuration = 0.3f; // Nhá trong bao lâu
    public float interval = 5f; // Khoảng cách giữa các lần nhá

    private Color originalColor;
    private Material deerMaterial;

    void Start()
    {
        if (deerRenderer == null)
            deerRenderer = GetComponent<Renderer>();

        if (deerRenderer != null)
        {
            deerMaterial = deerRenderer.material;
            originalColor = deerMaterial.color;
            StartCoroutine(FlashLoop());
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Renderer cho nai giả.");
        }
    }

    IEnumerator FlashLoop()
    {
        while (true)
        {
            // Đổi sang màu đỏ
            deerMaterial.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            // Trở lại màu ban đầu
            deerMaterial.color = originalColor;

            yield return new WaitForSeconds(interval - flashDuration);
        }
    }
}
