using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class HomeSetting : MonoBehaviour
{
    [SerializeField] private Button _huongDan;
    [SerializeField] private Button _btnSetting;
    [SerializeField] private Button _btnExit;
    [SerializeField] private Button _btnChoiMoi;
    [SerializeField] private Button _btnPhongTap;
    [SerializeField] private Button _btnBack;
    [SerializeField] private Button _btnDongHuongDan;
    [SerializeField] private Button _btnDongSetting;

    // Tên của các scene cần chuyển đổi
    [SerializeField] private string _phongTapSceneName; // Tên của scene tập luyện
    [SerializeField] private string _choiMoiSceneName; // Tên của moi
    [SerializeField] private string _backSceneName;


    [SerializeField] private TextMeshProUGUI _motaChoiMoi;
    [SerializeField] private TextMeshProUGUI _motaChoiLai;
    [SerializeField] private TextMeshProUGUI _motaPhongTap;
    [SerializeField] private TextMeshProUGUI _chaoMungText;
    [SerializeField] private TextMeshProUGUI _huongDanText;

    // canvas hiện lên sau khi nhấn setting
    [SerializeField] private GameObject _canvasHuongDan;
    [SerializeField] private GameObject _canvasSetting;

    // Toggle để bật tắt âm thanh nhạc nền
    [SerializeField] private Toggle _toggleMusic;
    [SerializeField] private Toggle _toggleLanguage; 
    [SerializeField] private AudioSource _backgroundMusic;

    [SerializeField] private GameObject _loadGame;




    // dữ liệu game
    private string _mota1 = "New level, fake deer hunting battle your mission is to protect real deer.";
    private string _mota2 = "Continue your old level.";
    private string _mota3 = "Practice your sniper skills with targets.";
    private string _huongDanChoiText = "You play the role of a farm owner and protect your herd of deer. Your task is to use a gun to shoot and destroy the deer that disguise themselves as thieves who sneak into your farm.\n R reload\nMouse Left Gun\nMouse Right to Scope";
    private string _mota1TiengViet = "Cấp độ mới, trận chiến săn hươu giả mạo nhiệm vụ của bạn là bảo vệ hươu.";
    private string _mota2TiengViet = "Tiếp tục cấp độ cũ của bạn.";
    private string _mota3TiengViet = "Luyện tập kỹ năng bắn tỉa của bạn với các mục tiêu.";
    private string _huongDanChoiTextTiengViet = "Bạn vào vai một chủ trang trại và bảo vệ đàn hươu của mình. Nhiệm vụ của bạn là sử dụng súng để bắn và tiêu diệt những con hươu giả mạo là kẻ trộm lẻn vào trang trại của bạn.\n R reload\nMouse Left Gun\nMouse Right to Scope";

    void Start()
    {
        SaveGamePlayerPrefs();
        LoadGamePlayerPrefs();

        _canvasHuongDan.SetActive(false);
        _canvasSetting.SetActive(false);
        _toggleMusic.isOn = true;
        _toggleLanguage.isOn = true; 
        _loadGame.SetActive(false);

        // Gán sự kiện cho nút 
        _huongDan.onClick.AddListener(OnHuongDanButtonClicked);
        _btnSetting.onClick.AddListener(OnSettingButtonClicked);
        _btnExit.onClick.AddListener(OnExitButtonClicked);
        _btnChoiMoi.onClick.AddListener(OnPlayNewGameClicked);
        _btnPhongTap.onClick.AddListener(OnPlayPhongTapClicked);
        _btnBack.onClick.AddListener(OnBackButtonClicked);
        _btnDongHuongDan.onClick.AddListener(OnDongHuongDanClicked);
        _btnDongSetting.onClick.AddListener(OnDongSettingClicked);
        ToggleMusic(); 

    }

    public void ToggleMusic()   
    {
        if (_toggleMusic.isOn)
        {
            _backgroundMusic.Play(); 
        }
        else
        {
            _backgroundMusic.Pause();
        }
    }
    public void ToggleLanguage()
    {
        if (_toggleLanguage.isOn)
        {
            LoadGamePlayerPrefs(); 
        }
        else
        {
            LoadGamePlayerPrefsTiengViet(); 
        }
    }



    void SaveGamePlayerPrefs()
    {
        PlayerPrefs.SetString("mota1", _mota1);
        PlayerPrefs.SetString("mota2", _mota2);
        PlayerPrefs.SetString("mota3", _mota3);
        PlayerPrefs.SetString("huongdanchoi", _huongDanChoiText);
        //lưu tiếng Việt
        PlayerPrefs.SetString("mota1TiengViet", _mota1TiengViet);
        PlayerPrefs.SetString("mota2TiengViet", _mota2TiengViet);
        PlayerPrefs.SetString("mota3TiengViet", _mota3TiengViet);
        PlayerPrefs.SetString("huongdanchoiTiengViet", _huongDanChoiTextTiengViet);
        PlayerPrefs.Save(); // Lưu thay đổi
        Debug.Log("Game saved to PlayerPrefs");
    }
    void LoadGamePlayerPrefs()
    {
        string mota1 = PlayerPrefs.GetString("mota1");
        string mota2 = PlayerPrefs.GetString("mota2");
        string mota3 = PlayerPrefs.GetString("mota3");
        string tenPlayer = PlayerPrefs.GetString("playerName");
        string textHuongDanChoi = PlayerPrefs.GetString("huongdanchoi");

        _motaChoiMoi.text = mota1; // Hiển thị mô tả chơi mới
        _motaChoiLai.text = mota2; // Hiển thị mô tả chơi lại
        _motaPhongTap.text = mota3; // Hiển thị mô tả phòng tập
        _chaoMungText.text = "Hi " + tenPlayer + ", please select game mode!";
        _huongDanText.text = textHuongDanChoi; // Hiển thị hướng dẫn chơi
    }

    void LoadGamePlayerPrefsTiengViet()
    {
        string mota1 = PlayerPrefs.GetString("mota1TiengViet");
        string mota2 = PlayerPrefs.GetString("mota2TiengViet");
        string mota3 = PlayerPrefs.GetString("mota3TiengViet");
        string tenPlayer = PlayerPrefs.GetString("playerName");
        string textHuongDanChoi = PlayerPrefs.GetString("huongdanchoiTiengViet");

        _motaChoiMoi.text = mota1; // Hiển thị mô tả chơi mới
        _motaChoiLai.text = mota2; // Hiển thị mô tả chơi lại
        _motaPhongTap.text = mota3; // Hiển thị mô tả phòng tập
        _chaoMungText.text = "Xin chào " + tenPlayer + ", vui lòng chọn chế độ chơi!";
        _huongDanText.text = textHuongDanChoi; // Hiển thị hướng dẫn chơi
    }



    // --------------------------------------thực hiện chuyển scene---------------------------------------------

    // Hàm xử lý khi nút Exit được nhấn
    private void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }
    //Sự kiện khi nút Chơi mới được nhấn
    private void OnPlayNewGameClicked()
    {
        Debug.Log("Chơi mới button clicked");
        _loadGame.SetActive(true);
        // Chuyển đến scene chơi mới
        StartCoroutine(Wait1s());
    }
    //Sự kiện khi nút Phòng tập được nhấn
    private void OnPlayPhongTapClicked()
    {
        Debug.Log("Phòng tập button clicked");
        // Chuyển đến scene phòng tập
        _loadGame.SetActive(true);
        StartCoroutine(Wait1sLoadPhongTap());
    }

    // Hàm xử lý khi nút Back được nhấn
    private void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked");
        SceneManager.LoadScene(_backSceneName);

    }





    // ---------------------------------------------thực hiện mở panel----------------------------------------------
    // Hàm xử lý khi nút Setting được nhấn
    private void OnSettingButtonClicked()
    {
        Debug.Log("Setting button clicked");
        _canvasSetting.SetActive(true);

    }
    private void OnHuongDanButtonClicked()
    {
        Debug.Log("Hướng dẫn button clicked");
        _canvasHuongDan.SetActive(true);

    }


    //--------------------------------đóng panel----------------------------------------------
    private void OnDongHuongDanClicked()
    {
        Debug.Log("Đóng hướng dẫn button clicked");
        _canvasHuongDan.SetActive(false);
    }
    private void OnDongSettingClicked()
    {
        Debug.Log("Đóng setting button clicked");
        _canvasSetting.SetActive(false);
    }

    IEnumerator Wait1s()
    {
        yield return new WaitForSeconds(1); // đợi 1 giây để hiển thị thông báo đăng nhập

        SceneManager.LoadScene(_choiMoiSceneName);

    }

    IEnumerator Wait1sLoadPhongTap()
    {
        yield return new WaitForSeconds(1); // đợi 1 giây để hiển thị thông báo đăng nhập

        SceneManager.LoadScene(_phongTapSceneName);

    }
}