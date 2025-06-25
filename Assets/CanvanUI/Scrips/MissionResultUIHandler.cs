using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MissionResultUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panelResult;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Buttons")]
    [SerializeField] private Button buttonChoilai;
    [SerializeField] private Button buttonVeMenu;
    [SerializeField] private Button buttonTiepTuc;

    [Header("Scene To Load")]
    [SerializeField] private string waitingRoomScene = "WaitingRoom";
    [SerializeField] private string mainMenuScene = "MainMenu";

    [Header("Loading Screen (Optional)")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;


    private void Awake()
    {
        if (panelResult != null)
            panelResult.SetActive(false);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }


    private void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingText != null)
                loadingText.text = $"Loading {Mathf.RoundToInt(progress * 100f)}%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    public void ShowResultUI(bool isWin)
    {
        if (panelResult != null)
            panelResult.SetActive(true);

        resultText.text = isWin ? "Winner" : "You Lose";

        buttonTiepTuc.gameObject.SetActive(true);
        buttonChoilai.gameObject.SetActive(true);
        buttonVeMenu.gameObject.SetActive(true);

        buttonTiepTuc.onClick.RemoveAllListeners();
        buttonChoilai.onClick.RemoveAllListeners();
        buttonVeMenu.onClick.RemoveAllListeners();

        if (isWin)
        {
            // Tăng level khi thắng
            LevelManager.Instance.NextLevel();
        }

        buttonTiepTuc.onClick.AddListener(() => LoadSceneWithLoading(waitingRoomScene));
        buttonChoilai.onClick.AddListener(() => LoadSceneWithLoading(SceneManager.GetActiveScene().name));
        buttonVeMenu.onClick.AddListener(() => LoadSceneWithLoading(mainMenuScene));

        LevelManager.playerJustWon = isWin;
        LevelManager.playerJustLost = !isWin;
    }
}
