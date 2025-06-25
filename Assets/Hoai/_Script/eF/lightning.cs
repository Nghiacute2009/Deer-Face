using UnityEngine;
using System.Collections;

public class lightning : MonoBehaviour
{
    [SerializeField] private Light lightningLight;
    [SerializeField] private AudioSource thunderSound;
    [SerializeField] private GameObject thunderVFX;

    [SerializeField] private float lightningDuration = 0.3f;
    [SerializeField] private float lightningChance = 0.005f;

    [SerializeField] private float minThunderDelay = 2f; // Sét xong 2–4 giây mới có sấm
    [SerializeField] private float maxThunderDelay = 4f;

    void Update()
    {
        if (Random.value < lightningChance)
        {
            StartCoroutine(TriggerLightning());
        }
    }

    IEnumerator TriggerLightning()
    {
        // Bật ánh sáng sét và hiệu ứng
        lightningLight.enabled = true;
        if (thunderVFX != null)
            thunderVFX.SetActive(true);

        // Giữ sét trong vài giây
        yield return new WaitForSeconds(lightningDuration);

        // Tắt ánh sáng và hiệu ứng
        lightningLight.enabled = false;
        if (thunderVFX != null)
            thunderVFX.SetActive(false);

        // Gọi coroutine riêng để phát âm thanh sau vài giây
        StartCoroutine(PlayThunderWithDelay());
    }

    IEnumerator PlayThunderWithDelay()
    {
        float delay = Random.Range(minThunderDelay, maxThunderDelay);
        yield return new WaitForSeconds(delay);

        thunderSound.Play();
    }
}
