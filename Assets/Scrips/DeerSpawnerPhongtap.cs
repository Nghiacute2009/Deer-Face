using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSpawnerPhongtap : MonoBehaviour
{
    [Header("Prefab con nai")]
    [SerializeField] private GameObject deerPrefab;

    [Header("Các điểm spawn (Transform đặt sẵn trong scene)")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Cài đặt")]
    [SerializeField] private float deerLifetime = 5f;     // Thời gian nai tồn tại
    [SerializeField] private float spawnInterval = 6f;    // Chu kỳ spawn
    [SerializeField] private KeyCode toggleKey = KeyCode.T;

    private List<GameObject> activeDeer = new List<GameObject>();
    private bool isSpawning = false;
    private Coroutine spawnCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!isSpawning)
            {
                isSpawning = true;
                spawnCoroutine = StartCoroutine(AutoSpawnLoop());
            }
            else
            {
                isSpawning = false;
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }

                foreach (var deer in activeDeer)
                {
                    Destroy(deer);
                }
                activeDeer.Clear();
            }
        }
    }

    IEnumerator AutoSpawnLoop()
    {
        while (isSpawning)
        {
            foreach (var deer in activeDeer)
            {
                Destroy(deer);
            }
            activeDeer.Clear();

            int deerCount = Random.Range(2, 5); // từ 2 đến 4 con (luôn < 5)
            List<Transform> availableSpawns = new List<Transform>(spawnPoints);

            for (int i = 0; i < deerCount && availableSpawns.Count > 0; i++)
            {
                int index = Random.Range(0, availableSpawns.Count);
                Transform spawnPoint = availableSpawns[index];
                availableSpawns.RemoveAt(index);

                GameObject newDeer = Instantiate(deerPrefab, spawnPoint.position, Quaternion.identity);
                activeDeer.Add(newDeer);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
