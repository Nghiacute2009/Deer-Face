using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPCStartMission : MonoBehaviour
{
    [Header("NPC Interaction")]
    public GameObject missionUI;
    public TextMeshProUGUI dialogueText;
    public float interactionDistance = 5f;
    public Transform player;

    [Header("Scene Loading")]
    public GameObject loadingPanel;
    public TextMeshPro loadingText;

    [Header("Greeting UI")]
    [SerializeField] private GameObject greetingPanel;
    [SerializeField] private TextMeshProUGUI greetingText;
    [SerializeField] private float greetingStepTime = 6f;

    private bool isInRange = false;
    private Quaternion originalRotation;

    public static bool playerJustWon = false;
    public static bool playerJustLost = false;

    private readonly string[] greetingSteps = new string[]
    {
        "", // bỏ index 0
        "Chào {0}!\nChào mừng đến với 'phòng chờ'!",
        "Đây là khu vực chờ để bạn làm quen thao tác.",
        "Có bia tập bắn để bạn ngắm bắn.",
        "Hãy thử di chuyển, ngắm bắn, thay đạn.",
        "Khi đã sẵn sàng, tiến lại gần NPC và 'nhấn Enter' để bắt đầu nhiệm vụ."
    };

    private void Start()
    {
        originalRotation = transform.rotation;
        missionUI.SetActive(false);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        ShowGreeting();
    }

    void Update()
    {
        if (greetingPanel != null && greetingPanel.activeSelf)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        LookAtPlayer();

        if (isInRange)
        {
            missionUI.SetActive(true);
            UpdateDialogue();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadSceneWithLoading("MainScene"));
            }
        }
        else
        {
            missionUI.SetActive(false);
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    void UpdateDialogue()
    {
        if (dialogueText == null) return;

        string playerName = PlayerPrefs.GetString("playerName", "người chơi");

        if (playerJustWon)
        {
            dialogueText.text = $"Chúc mừng {playerName} đã chiến thắng! Sẵn sàng cho thử thách tiếp theo chứ?";
            playerJustWon = false;
        }
        else if (playerJustLost)
        {
            dialogueText.text = $"{playerName}, bạn đã thua, hãy cố gắng hơn ở lần sau!";
            playerJustLost = false;
        }
        else
        {
            dialogueText.text = $"Chào {playerName}! Nhấn Enter để bắt đầu nhiệm vụ.";
        }
    }

    void ShowGreeting()
    {
        string playerName = PlayerPrefs.GetString("playerName", "người chơi");

        if (LevelManager.playerJustWon)
        {
            greetingText.text = $"Chúc mừng {playerName} đã chiến thắng!\nSẵn sàng cho thử thách tiếp theo chứ?";
            greetingPanel.SetActive(true);
            LevelManager.playerJustWon = false; // Reset sau khi hiện
            StartCoroutine(HideGreetingAfterSeconds(5f));
        }
        else if (LevelManager.playerJustLost)
        {
            greetingText.text = $"{playerName}, bạn đã thua, hãy cố gắng hơn ở lần sau!";
            greetingPanel.SetActive(true);
            LevelManager.playerJustLost = false; // Reset sau khi hiện
            StartCoroutine(HideGreetingAfterSeconds(5f));
        }
        else
        {
            // Chào người mới
            StartCoroutine(ShowGreetingSequence());
        }
    }

    IEnumerator HideGreetingAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        greetingPanel.SetActive(false);
    }


    IEnumerator ShowGreetingSequence()
    {
        string playerName = PlayerPrefs.GetString("playerName", "người chơi");

        for (int i = 1; i <= 5; i++)
        {
            string message = string.Format(greetingSteps[i], playerName);
            greetingText.text = message;

            float elapsed = 0f;
            bool skipped = false;

            while (elapsed < greetingStepTime)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    skipped = true;
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!skipped)
                yield return new WaitForSeconds(greetingStepTime - elapsed);
        }

        greetingPanel.SetActive(false);
    }

    IEnumerator LoadSceneWithLoading(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingText != null)
                loadingText.text = $"Đang tải: {(progress * 100f):0}%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
