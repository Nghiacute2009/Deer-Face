using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textKetQuaLogin;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TextMeshProUGUI _thongBaoChaoMung;
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private TextMeshProUGUI _textLoading; // Thông báo đang đăng nhập
    [SerializeField] private Slider mySlider;
    public float fillDuration = 2f;
    public string nextSceneName = "MainScene";

    void Start()
    {
        mySlider.value = 0;
        mySlider.gameObject.SetActive(false);
        _textLoading.text = "0%"; // Khởi tạo giá trị slider

    }

    // Update is called once per frame
    void Update()
    {
        _textLoading.text = ((int)mySlider.value).ToString() + "%"; // Cập nhật giá trị slider
        //nhấn enter để đăng nhập
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (string.IsNullOrWhiteSpace(_nameInputField.text) || _nameInputField.text.Length >= 15)//kiểm tra tên có rỗng hoặc nhiều hơn 20 ký tự
            {
                _textKetQuaLogin.text = "Vui lòng nhập tên nhân vật!";
            }
            else
            {
                _textKetQuaLogin.text = "Đang đăng nhập...";
                SaveGamePlayerPrefs();
                StartCoroutine(Wait1s());
            }

        }
    }

    IEnumerator FillSliderOverTime()
    {

        mySlider.gameObject.SetActive(true); //hiện lại slider loading
        float elapsed = 0f;
        float pausePoint = 0.8f; // 80%
        float pauseTime = fillDuration * pausePoint;
        bool paused = false;

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fillDuration;
            t = Mathf.Clamp01(t);

            mySlider.value = Mathf.Lerp(0f, mySlider.maxValue, t);

            // Nếu chưa pause và đã đến 80%
            if (!paused && t >= pausePoint)
            {
                paused = true;
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }

        mySlider.value = mySlider.maxValue;

        // Chuyển scene sau khi đầy
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Wait1s()
    {
        yield return new WaitForSeconds(1); // đợi 1 giây để hiển thị thông báo đăng nhập

        string namePlyer = PlayerPrefs.GetString("playerName");
        _backgroundImage.SetActive(true); // hiện màn loading
        _thongBaoChaoMung.text = "Chào " + namePlyer + ", chúc bạn chơi game vui!";

        StartCoroutine(FillSliderOverTime());

    }

    //Lưu tên người chơi vào PlayerPrefs
    void SaveGamePlayerPrefs()
    {
        PlayerPrefs.SetString("playerName", _nameInputField.text);
        PlayerPrefs.Save(); // Lưu thay đổi
        Debug.Log("Game saved to PlayerPrefs");
    }


}
