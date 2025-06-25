using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerShow : MonoBehaviour
{



 


    public float fadeDuration = 2f; // thời gian mờ dần
    private Material[] allMats;

    void Start()
    {
        // Lấy tất cả materials có trong Renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        allMats = renderers.SelectMany(r => r.materials).ToArray();

        // Đặt shader thành Transparent trước khi hiển thị
        foreach (Material mat in allMats)
        {
            if (mat.HasProperty("_Color"))
            {
                mat.SetFloat("_Mode", 2); // Transparent mode nếu dùng Standard shader
                mat.SetOverrideTag("RenderType", "Transparent");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                Color c = mat.color;
                c.a = 0f;
                mat.color = c;
            }
        }

        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            foreach (Material mat in allMats)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color c = mat.color;
                    c.a = alpha;
                    mat.color = c;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha = 1 khi xong
        foreach (Material mat in allMats)
        {
            if (mat.HasProperty("_Color"))
            {
                Color c = mat.color;
                c.a = 1f;
                mat.color = c;
            }

            // Chuyển shader về Opaque
            mat.SetOverrideTag("RenderType", "");
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;
        }
    }
}

    
    



