using UnityEngine;

public class WaitingRoomHandler : MonoBehaviour
{
    void Start()
    {
        if (LevelManager.playerJustWon)
        {
            LevelManager.playerJustWon = false;
            LevelManager.Instance.NextLevel();
            Debug.Log("Sang level tiếp theo");
        }
        else if (LevelManager.playerJustLost)
        {
            LevelManager.playerJustLost = false;
            LevelManager.Instance.Restart();
            Debug.Log("Quay lại level đầu");
        }
    }
}
