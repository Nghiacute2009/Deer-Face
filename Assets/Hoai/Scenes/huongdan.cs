using UnityEngine;
using TMPro;

public class huongdan : MonoBehaviour
{
    [SerializeField] private GameObject _panelHuongDan; // Panel hướng dẫn
    [SerializeField] private GameObject _buttonHuongDan; // Nút để mở panel hướng dẫn
    [SerializeField] private GameObject _buttonCloseHuongDan; // Nút để đóng panel hướng dẫn
    [SerializeField] private TextMeshProUGUI _textHuongDan; // Văn bản hướng dẫn
                                                            // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string _noidung;
    void Start()
    {
        //_noidung = "Hướng dẫn chơi game:\n" +
        //                "1. Sử dụng các phím mũi tên để di chuyển nhân vật.\n" +
        //                "2. Nhấn phím Space để tấn công.\n" +
        //                "3. Thu thập vật phẩm để tăng sức mạnh.\n" +
        //                "4. Tránh né kẻ thù và hoàn thành nhiệm vụ.\n" +
        //                "Chúc bạn chơi game vui vẻ!";
        //SaveGamePlayerPrefs(); // Lưu nội dung hướng dẫn vào PlayerPrefs    
        string noidungdocra = PlayerPrefs.GetString("huongdanchoiTiengViet");
        _textHuongDan.text = noidungdocra; // Hiển thị nội dung hướng dẫn
        _panelHuongDan.SetActive(false); // Ẩn panel hướng dẫn khi bắt đầu

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SaveGamePlayerPrefs()
    {
        PlayerPrefs.SetString("noidung", _noidung);
        PlayerPrefs.Save(); // Lưu thay đổi
        Debug.Log("Game saved to PlayerPrefs");
    }

    public void OnButtonHuongDanClick()
    {
        _panelHuongDan.SetActive(true); // Hiện panel hướng dẫn
        _buttonHuongDan.SetActive(false); // Ẩn nút mở panel hướng dẫn
        Time.timeScale = 0; // Tạm dừng game khi mở panel hướng dẫn
    }

    public void OnButtonCloseHuongDanClick()
    {
        _panelHuongDan.SetActive(false); // Ẩn panel hướng dẫn
        _buttonHuongDan.SetActive(true); // Hiện lại nút mở panel hướng dẫn
        Time.timeScale = 1; // Tiếp tục game khi đóng panel hướng dẫn
    }
}
