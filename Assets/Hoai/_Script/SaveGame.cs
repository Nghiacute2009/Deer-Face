using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


// định nghĩa dữ liệu lưu game
[Serializable]
public class GameData
{
    public int level;
    public int score;
    public string playerName;
    // thêm các trường dữ liệu khác nếu cần
}


public class SaveGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Đọc dữ liệu game khi bắt đầu
        // ReadGame(); // Gọi hàm đọc game khi bắt đầu
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // SaveGame(); // Gọi hàm lưu game khi nhấn phím A
            SaveGamePlayerPrefs(); // Lưu game vào PlayerPrefs  

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // ReadGame(); // Gọi hàm đọc game khi nhấn phím S
            ReadGamePlayerPrefs(); // Đọc game từ PlayerPrefs
        }






        // Hàm lưu game PlayerPrefs
        void SaveGamePlayerPrefs()
        {
            var gameData = new GameData
            {
                level = 10, // ví dụ, lấy từ biến hiện tại
                score = 1000, // ví dụ, lấy từ biến hiện tại
                playerName = "Player10" // ví dụ, lấy từ biến hiện tại
            };

            PlayerPrefs.SetInt("level", gameData.level); // ví dụ, lấy từ biến hiện tại
            PlayerPrefs.SetInt("score", 100); // ví dụ, lấy từ biến hiện tại
            PlayerPrefs.SetString("playerName", "Player1"); // ví dụ, lấy từ biến hiện tại
            // PlayerPrefs.SetString("gameData", JsonUtility.ToJson(gameData)); // Lưu dữ liệu game dưới dạng JSON
            PlayerPrefs.Save(); // Lưu thay đổi
            Debug.Log("Game saved to PlayerPrefs");
        }

        // Hàm đọc game từ PlayerPrefs
        void ReadGamePlayerPrefs()
        {
            if (PlayerPrefs.HasKey("level"))
            {
                int level = PlayerPrefs.GetInt("level");
                int score = PlayerPrefs.GetInt("score");
                string playerName = PlayerPrefs.GetString("playerName");
                // string gameDataJson = PlayerPrefs.GetString("gameData");
                // var gameData = JsonUtility.FromJson<GameData>(gameDataJson);


                Debug.Log($"Game loaded from PlayerPrefs: {playerName}, " +
                          $"Level: {level}, Score: {score}");
            }
            else
            {
                Debug.LogWarning("No game data found in PlayerPrefs");
            }
        }
    }
}
