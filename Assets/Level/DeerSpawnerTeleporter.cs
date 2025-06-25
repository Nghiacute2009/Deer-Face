using UnityEngine;
using UnityEngine.AI;

public class DeerSpawnerTeleporter : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject naiRealPrefab;
    public GameObject naiFakePrefab;

    [Header("NavMesh settings")]
    public float checkRadius = 2f;
    public int maxTries = 50;

    public void SpawnDeerFromCurrentLevel()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogError("❌ LevelManager.Instance = null");
            return;
        }

        LevelConfig level = LevelManager.Instance.GetCurrentLevel();
        Debug.Log($"📦 Bắt đầu Spawn nai - Level {LevelManager.Instance.currentLevelIndex + 1} | Nai thật: {level.naiReal}, Nai giả: {level.naiFake}");

        SpawnDeer(level.naiReal, level.naiFake, level.teleportAreaCenter, level.teleportAreaSize);
    }

    private void SpawnDeer(int naiRealCount, int naiFakeCount, Vector3 center, Vector3 size)
    {
        if (naiRealPrefab == null || naiFakePrefab == null)
        {
            Debug.LogError("❌ Chưa gán prefab naiReal hoặc naiFake trong Inspector!");
            return;
        }

        int realSpawned = 0;
        int fakeSpawned = 0;

        for (int i = 0; i < naiRealCount; i++)
        {
            Vector3 pos = GetRandomNavMeshPosition(center, size);
            if (pos != Vector3.zero)
            {
                SpawnAt(pos, naiRealPrefab);
                realSpawned++;
            }
        }

        for (int i = 0; i < naiFakeCount; i++)
        {
            Vector3 pos = GetRandomNavMeshPosition(center, size);
            if (pos != Vector3.zero)
            {
                SpawnAt(pos, naiFakePrefab);
                fakeSpawned++;
            }
        }

        Debug.Log($"✅ Đã spawn: {realSpawned}/{naiRealCount} nai thật, {fakeSpawned}/{naiFakeCount} nai giả.");
    }

    private void SpawnAt(Vector3 position, GameObject prefab)
    {
        GameObject deer = Instantiate(prefab, position, Quaternion.identity);

        // Gán nhiệm vụ nếu có
        var butter = deer.GetComponent<ControllerButter>();
        if (butter != null)
            butter.NhiemVu = FindObjectOfType<NhiemVu>();

        var butterR = deer.GetComponent<ControllerButterR>();
        if (butterR != null)
            butterR.NhiemVu = FindObjectOfType<NhiemVu>();
    }

    private Vector3 GetRandomNavMeshPosition(Vector3 center, Vector3 size)
    {
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 randomPos = center + new Vector3(
                Random.Range(-size.x / 2, size.x / 2),
                0,
                Random.Range(-size.z / 2, size.z / 2)
            );

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, checkRadius, NavMesh.AllAreas))
                return hit.position;
        }

        Debug.LogWarning("⚠️ Không tìm được vị trí NavMesh hợp lệ!");
        return Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (LevelManager.Instance != null)
        {
            LevelConfig level = LevelManager.Instance.GetCurrentLevel();
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(level.teleportAreaCenter, level.teleportAreaSize);
        }
    }
}
