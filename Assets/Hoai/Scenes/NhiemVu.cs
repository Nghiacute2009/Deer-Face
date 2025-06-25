// Phiên bản hoàn chỉnh của script nhiệm vụ NPC với hệ thống thoại theo từng câu, giữ chức năng cũ và thêm chức năng mới theo yêu cầu

using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NhiemVu : MonoBehaviour
{
    [Header("NPC & Player")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform viTriCuaPlayer;
    [SerializeField] Animator animator;
    [SerializeField] private int countdownStartSeconds = 3;

    [Header("UI References")]
    [SerializeField] GameObject hoiThoaiGiuaPlayerVaNPC;
    [SerializeField] GameObject batDauNhiemVu;
    [SerializeField] GameObject hienCanhBao;
    [SerializeField] GameObject hienGiet1ConNaiGia;
    [SerializeField] GameObject thoiGianKhiBAtDauNhiemVu;
    [SerializeField] TextMeshProUGUI textHoiThoaiNPC;
    [SerializeField] TextMeshProUGUI textCanhBao;
    [SerializeField] TextMeshProUGUI textGietNai;
    [SerializeField] TextMeshProUGUI textSoConNai;
    [SerializeField] TextMeshProUGUI textThoiGianNhiemVu;
    [SerializeField] TextMeshProUGUI textSoNaiRealDaGiet;
    [SerializeField] TextMeshProUGUI textCountdownStart;
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] TextMeshProUGUI textTimePerLevel;
    [SerializeField] RawImage effectScreenRed;
    [SerializeField] private MissionResultUIHandler missionResultUIHandler;

    [Header("Nhiệm Vụ Settings")]
    public float khoangCachDeBatDauHoiThoai = 5f;
    public int soConNaiFake = 3;
    public int soConNaiReal = 7;
    public float thoiGianNhiemVu = 180f;
    public float tocDoThoiGian = 1f;

    private int soNaiRealDaBiGiet = 0;
    private bool daBatDauNhiemVu = false;
    private Coroutine countdownCoroutine;
    private bool daCanhBao30s = false;
    private Coroutine demNguoc10sCoroutine = null;

    private List<string> dialogueLines = new List<string>();
    private int currentDialogueIndex = 0;
    private bool isTalking = false;
    private int currentLevel = 1;

    void Start()
    {
        hoiThoaiGiuaPlayerVaNPC.SetActive(false);
        batDauNhiemVu.SetActive(false);
        hienCanhBao.SetActive(false);
        hienGiet1ConNaiGia.SetActive(false);
        thoiGianKhiBAtDauNhiemVu.SetActive(false);
        textCountdownStart?.gameObject.SetActive(false);

        if (LevelManager.Instance != null)
        {
            var config = LevelManager.Instance.GetCurrentLevel();
            soConNaiFake = config.naiFake;
            soConNaiReal = config.naiReal;
            thoiGianNhiemVu = config.thoiGian;
            currentLevel = LevelManager.Instance.currentLevelIndex + 1;
            textLevel.text = $"Level: {currentLevel}";
            textTimePerLevel.text = $"Thời gian cấp độ: {config.thoiGian} giây";
        }

        CapNhatUI();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, viTriCuaPlayer.position);

        if (distance <= khoangCachDeBatDauHoiThoai && !daBatDauNhiemVu)
        {
            transform.LookAt(new Vector3(viTriCuaPlayer.position.x, transform.position.y, viTriCuaPlayer.position.z));
            animator.SetBool("GapPlayer", true);

            if (!isTalking)
                StartDialogue();

            if (Input.GetKeyDown(KeyCode.Return))
                ContinueDialogue();

            if (Input.GetKeyDown(KeyCode.Backspace))
                CancelDialogue();
        }
        else
        {
            CloseDialogue();
            animator.SetBool("GapPlayer", false);
        }

        if (daBatDauNhiemVu)
        {
            thoiGianNhiemVu -= Time.deltaTime * tocDoThoiGian;
            if (thoiGianNhiemVu <= 0)
            {
                thoiGianNhiemVu = 0;
                daBatDauNhiemVu = false;
            }
            else if (thoiGianNhiemVu <= 30 && !daCanhBao30s)
            {
                daCanhBao30s = true;
                StartCoroutine(CanhBao30s());
            }
            else if (thoiGianNhiemVu <= 10 && demNguoc10sCoroutine == null)
            {
                demNguoc10sCoroutine = StartCoroutine(CanhBao10s());
            }
        }

        CapNhatUI();
        KiemTraCanhBao();
        KiemTraKetQua();
    }

    public void StartDialogue()
    {
        string playerName = PlayerPrefs.GetString("playerName", "Người chơi");
        currentDialogueIndex = 0;
        isTalking = true;
        hoiThoaiGiuaPlayerVaNPC.SetActive(true);

        if (currentLevel == 1)
        {
            dialogueLines = new List<string>
            {
                $"Xin chào {playerName}!",
                $"Nhiệm vụ đầu tiên của bạn là tiêu diệt {soConNaiFake} con nai giả trong tổng số {soConNaiFake + soConNaiReal} con.",
                $"Nai giả sẽ nhá đỏ sau mỗi 10s, hãy chú ý tránh giết nhầm nai thật,chúc bạn may mắn!",
                $"Nhấn [Enter] để bắt đầu nhiệm vụ."
            };
        }
        else
        {
            dialogueLines = new List<string>
            {
                $"Xin chào {playerName}!",
                $"Chào mừng đến với nhiệm vụ thứ {currentLevel}!",
                $"Bạn cần tiêu diệt {soConNaiFake} con nai giả trong tổng số {soConNaiFake + soConNaiReal} con nai.",
                $"Tránh giết nhầm nai thật, chúc bạn may mắn!.",
                $"Nhấn [Enter] thêm lần nữa để bắt đầu nhiệm vụ."
            };
        }

        textHoiThoaiNPC.text = dialogueLines[currentDialogueIndex];
    }

    public void ContinueDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueLines.Count)
        {
            textHoiThoaiNPC.text = dialogueLines[currentDialogueIndex];
        }
        else
        {
            hoiThoaiGiuaPlayerVaNPC.SetActive(false);
            isTalking = false;
            StartCoroutine(BatDauSau(countdownStartSeconds));
        }
    }

    public void CancelDialogue() => CloseDialogue();

    public void CloseDialogue()
    {
        hoiThoaiGiuaPlayerVaNPC.SetActive(false);
        isTalking = false;
    }

    void CapNhatUI()
    {
        if (textThoiGianNhiemVu != null)
        {
            int m = Mathf.FloorToInt(thoiGianNhiemVu / 60);
            int s = Mathf.FloorToInt(thoiGianNhiemVu % 60);
            textThoiGianNhiemVu.text = $"Thời gian: {m:00}:{s:00}";
        }

        if (textSoConNai != null && LevelManager.Instance != null)
        {
            int naiFakeDaGiet = LevelManager.Instance.GetCurrentLevel().naiFake - soConNaiFake;
            int tong = LevelManager.Instance.GetCurrentLevel().naiFake;
            textSoConNai.text = $"Nai giả: {naiFakeDaGiet}/{tong}";
        }

        if (textSoNaiRealDaGiet != null)
            textSoNaiRealDaGiet.text = $"Nai thật: {soNaiRealDaBiGiet}";
    }

     public IEnumerator BatDauSau(int delay)
    {
        textCountdownStart.gameObject.SetActive(true);
        for (int i = delay; i > 0; i--)
        {
            textCountdownStart.text = $"Bắt đầu sau\n{i}";
            yield return new WaitForSeconds(1);
        }
        textCountdownStart.gameObject.SetActive(false);
        batDauNhiemVu?.SetActive(true);
        thoiGianKhiBAtDauNhiemVu?.SetActive(true);
        daBatDauNhiemVu = true;
        FindObjectOfType<DeerSpawnerTeleporter>()?.SpawnDeerFromCurrentLevel();
    }

     public IEnumerator CanhBao30s()
    {
        textCanhBao.text = "Còn 30 giây! Hãy nhanh lên!";
        hienCanhBao.SetActive(true);
        yield return new WaitForSeconds(5);
        hienCanhBao.SetActive(false);
    }

    public IEnumerator CanhBao10s()
    {
        for (int i = 10; i >= 1; i--)
        {
            textCanhBao.text = $"Còn: {i} giây";
            hienCanhBao.SetActive(true);
            yield return new WaitForSeconds(1);
        }
        hienCanhBao.SetActive(false);
    }

    public void KiemTraCanhBao()
    {
        if (soNaiRealDaBiGiet == 1)
        {
            textCanhBao.text = "Cẩn thận! Bạn đã giết 1 nai thật. Còn 2 cơ hội.";
            StartCoroutine(HienCanhBaoTrong(3f));
        }
        else if (soNaiRealDaBiGiet == 2)
        {
            textCanhBao.text = "Nguy hiểm! Bạn đã giết 2 nai thật. Giết thêm là thua!";
            StartCoroutine(HienCanhBaoTrong(3f));
        }
    }   
    public IEnumerator HienCanhBaoTrong(float seconds)
    {
        hienCanhBao.SetActive(true);
        yield return new WaitForSeconds(seconds);
        hienCanhBao.SetActive(false);
    }


    public void KiemTraKetQua()
    {
        bool hoanThanh = (soConNaiFake == 0 && soConNaiReal >= 1 && thoiGianNhiemVu > 0);
        bool thatBai = (soConNaiReal == 0 && soConNaiFake >= 1) || (thoiGianNhiemVu <= 0 && soConNaiFake > 0) || (soNaiRealDaBiGiet >= 3);

        if (hoanThanh)
        {
            daBatDauNhiemVu = false;
            missionResultUIHandler.ShowResultUI(true);
        }
        else if (thatBai)
        {
            daBatDauNhiemVu = false;
            missionResultUIHandler.ShowResultUI(false);
        }
    }


    public void GietNaiFake() => soConNaiFake--;
    public void GietNaiReal() { soConNaiReal--; soNaiRealDaBiGiet++; StartCoroutine(FlashRed()); }
    public void TruThoiGian() { thoiGianNhiemVu -= 2f; if (thoiGianNhiemVu < 0) thoiGianNhiemVu = 0f; }

     public IEnumerator FlashRed()
    {
        Color c = effectScreenRed.color;
        float duration = 0.3f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 0.5f, t / duration);
            effectScreenRed.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0.5f, 0, t / duration);
            effectScreenRed.color = c;
            yield return null;
        }
    }
}