using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class Setting : MonoBehaviour
{
    [SerializeField] private Button _buttonSetting;
    [SerializeField] private GameObject _panelSetting;
    [SerializeField] private Toggle _toggleMusic;
    [SerializeField] private AudioSource _backgroundMusic; // Thêm AudioSource để quản lý nhạc nền

    [SerializeField] private string _sceneName; // Tên scene để quay về
    [SerializeField] private GameObject _load;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _panelSetting.SetActive(false);
        _toggleMusic.isOn = true; // Mặc định bật nhạc
        _backgroundMusic.Play();
        _load.SetActive(false);

    }

    // Update is called once per frames
    void Update()
    {

    }

    //click toggle để bật tắt nhạc
    public void OnToggleMusic()
    {
        if (_toggleMusic.isOn)
        {
            // Bật nhạc
            Debug.Log("Nhạc đã được bật");

            _backgroundMusic.Play(); // Phát nhạc nền
            // Thêm mã để bật nhạc ở đây
        }
        else
        {
            // Tắt nhạc
            Debug.Log("Nhạc đã được tắt");
            _backgroundMusic.Stop(); // Dừng nhạc nền
            // Thêm mã để tắt nhạc ở đây
        }
    }
    public void OnButtonSettingClick()
    {
        _panelSetting.SetActive(true); // Hiện hoặc ẩn panel setting
        _buttonSetting.interactable = false; // Vô hiệu hóa nút setting khi panel đang mở
        Time.timeScale = 0; // Tạm dừng game
    }

    public void OnClosePanelSetting()
    {
        _panelSetting.SetActive(false); // Ẩn panel setting
        _buttonSetting.interactable = true; // Kích hoạt lại nút setting
        Time.timeScale = 1; // Tạm dừng game
    }

    public void OnExitGame()
    {
        _load.SetActive(true);
        _panelSetting.SetActive(false);
        Debug.Log("thottttttt");
        StartCoroutine(Wait1s());
        SceneManager.LoadScene(_sceneName);

    }

    public void OnResetGame()
    {
        Debug.Log("Đang reset game");
        Time.timeScale = 1; // Đảm bảo game được tiếp tục sau khi reset
        _load.SetActive(true);
        _panelSetting.SetActive(false);
        Debug.Log("choilai");
           new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Tải lại scene hiện tại để reset game

    }

    public void StopAndPlayGame()
    {
        int solanbam = 0;
        if(solanbam == 0)
        {
            Debug.Log("Đang tạm dừng game");
            Time.timeScale = 0; // Tạm dừng game
            solanbam =1 ;
        }
        else if(solanbam == 1)
        {
            Debug.Log("Đang tiếp tục game");
            Time.timeScale = 1; // Tiếp tục game
            solanbam = 0;
        }
    }

    IEnumerator Wait1s()
    {
        yield return new WaitForSeconds(4); // đợi 1 giây để hiển thị thông báo đăng nhập

        

    }


}
