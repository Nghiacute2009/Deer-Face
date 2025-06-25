using UnityEngine;

public class Spaw : MonoBehaviour
{

    [SerializeField] private GameObject spawnEffectPrefab;

    void Start()
    {
        if (spawnEffectPrefab != null)
        {
            GameObject effect = Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f); // tự hủy sau 2s để không chiếm bộ nhớ

            Vector3 effectOffset = new Vector3(0, -1f, 0); // Giảm trục Y để hiệu ứng nằm dưới
Instantiate(spawnEffectPrefab, transform.position + effectOffset, Quaternion.identity);

        }
    }
}

        
