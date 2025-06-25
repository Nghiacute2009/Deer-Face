using UnityEngine;

public class NPCRandomAnimation : MonoBehaviour
{
    public Animator animator;
    public float minInterval = 5f;
    public float maxInterval = 10f;

    private bool isRandomPlaying = false;

    void Start()
    {
        StartCoroutine(LoopRandomAnimations());
    }

    System.Collections.IEnumerator LoopRandomAnimations()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            if (!isRandomPlaying)
            {
                // Chọn random Buff (1), Casting (2), hoặc Stunned (3)
                int anim = Random.Range(1, 4);
                animator.SetInteger("AnimIndex", anim);
                isRandomPlaying = true;

                // Chờ khoảng thời gian animation phụ
                yield return new WaitForSeconds(5f);

                // Reset lại về Idle
                animator.SetInteger("AnimIndex", 0);
                isRandomPlaying = false;
            }
        }
    }
}
