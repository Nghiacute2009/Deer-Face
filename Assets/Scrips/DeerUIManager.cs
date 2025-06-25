using UnityEngine;
using TMPro;

public class DeerUIManager : MonoBehaviour
{
    public static DeerUIManager Instance;

    [Header("UI Hiển thị")]
    public TextMeshProUGUI deerCountText;     // Hiển thị số lượng hươu
    public TextMeshProUGUI complimentText;
    [SerializeField] private GameObject greetingPanel;// Hiển thị lời khen

    private int deerShot = 0;
    private bool hasComplimented30 = false;
    private bool hasComplimented50 = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void IncreaseDeerCount()
    {
        deerShot++;
        UpdateUI();
        CheckCompliments();
    }

    private void UpdateUI()
    {
        if (deerCountText != null)
            deerCountText.text = "Số hươu: " + deerShot;
    }

    private void CheckCompliments()
    {
        if (deerShot >= 30 && !hasComplimented30)
        {
            hasComplimented30 = true;
            ShowCompliment("Giỏi lắm! Bạn đã bắn được 30 con hươu!");
        }

        if (deerShot >= 50 && !hasComplimented50)
        {
            hasComplimented50 = true;
            ShowCompliment("Tuyệt vời! 50 con hươu đã gục ngã!");
        }
    }

    private void ShowCompliment(string message)
    {
        if (complimentText == null) return;

        complimentText.text = message;
        complimentText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideCompliment)); // Đảm bảo không trùng invoke
        Invoke(nameof(HideCompliment), 4f);   // Tự ẩn sau 4 giây
    }

    private void HideCompliment()
    {
        if (complimentText != null)
            complimentText.gameObject.SetActive(false);
    }
}
