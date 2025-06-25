using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static bool playerJustWon = false;
    public static bool playerJustLost = false;


    public List<LevelConfig> levels = new List<LevelConfig>();
    public int currentLevelIndex = 0;

    [SerializeField] private int totalLevelsToGenerate = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateLevels(); // <== Tạo tự động ở đây
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void GenerateLevels()
    {
        float groundHeight = 20f; // Điều chỉnh đúng với địa hình của bạn (nếu cần)

        Vector3 center = new Vector3(150, groundHeight, 162); // Y cao hơn 0

        for (int i = 0; i < totalLevelsToGenerate; i++)
        {
            int fake = Mathf.Clamp(7 - i, 1, 7);
            int real = Mathf.Clamp(3 + i, 3, 10);
            float time = Mathf.Clamp(180 - i * 2, 20, 180);
            float interval = Mathf.Clamp(10f - i, 3f, 10f);

            Vector3 spawnSize = new Vector3(40 + i * 10, 20, 40 + i * 10); // Tăng chiều cao vùng spawn lên 5

            LevelConfig config = new LevelConfig
            {
                naiFake = fake,
                naiReal = real,
                thoiGian = time,
                teleportInterval = interval,
                teleportAreaCenter = center,
                teleportAreaSize = spawnSize
            };

            levels.Add(config);
        }

        Debug.Log($"Đã tạo {levels.Count} cấp độ tự động!");
    }


    public LevelConfig GetCurrentLevel()
    {
        Debug.Log($"Lấy dữ liệu level hiện tại: Level {currentLevelIndex}");
        return levels[currentLevelIndex];
    }

    public bool HasNextLevel()
    {
        bool result = currentLevelIndex < levels.Count - 1;
        Debug.Log($"Có level tiếp theo không? {result}");
        return result;
    }

    public void NextLevel()
    {
        if (HasNextLevel())
        {
            currentLevelIndex++;
            Debug.Log($" Chuyển sang Level {currentLevelIndex}");
        }
        else
        {
            Debug.LogWarning("Không còn level tiếp theo.");
        }
    }

    public void Restart()
    {
        currentLevelIndex = 0;
        Debug.Log("Restart về Level 0");
    }
}
