using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WelcomeMessageNPC : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject greetingPanel;
    [SerializeField] private TextMeshProUGUI greetingText;
    [SerializeField] private float greetingDuration = 4f; // thời gian mỗi câu

    [Header("Người chơi")]
    [SerializeField] private Transform player;

    [Header("Danh sách lời chào")]
    [TextArea]
    [SerializeField]
    private List<string> greetingMessages = new List<string>
    {
        "Chào mừng {name}\n Đến với phòng tập!",
        "{name}! \n Bạn đã sẵn sàng luyện tập chưa?",
        "{name}, hãy ấn T khi bạn đã sẵn sàng",
        "Rất vui được gặp bạn, {name}!",
        "Chúc bạn luyện tập hiệu quả, {name}!"
    };

    private string playerName;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("playerName", "người chơi");

        if (greetingMessages.Count > 0)
            StartCoroutine(ShowAllGreetings());
    }

    private void Update()
    {
        LookAtPlayer();
    }

    private IEnumerator ShowAllGreetings()
    {
        if (greetingPanel == null || greetingText == null) yield break;

        greetingPanel.SetActive(true);

        foreach (string rawMsg in greetingMessages)
        {
            string message = rawMsg.Replace("{name}", playerName);
            greetingText.text = message;

            yield return new WaitForSeconds(greetingDuration);
        }

        greetingPanel.SetActive(false);
    }

    private void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
