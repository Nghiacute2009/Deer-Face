using UnityEngine;
using System.Collections;
public class deerKeu : MonoBehaviour
{
    [SerializeField] private AudioSource enemyAudio;

    void Start()
    {
        if (enemyAudio != null)
        {
            enemyAudio.loop = false; // đảm bảo âm thanh không tự lặp
            StartCoroutine(RepeatSound());
        }
    }

    IEnumerator RepeatSound()
    {
        while (true)
        {
            enemyAudio.Play();
            yield return new WaitForSeconds(enemyAudio.clip.length + 5f); // đợi phát xong + 5 giây
        }
    }
}
