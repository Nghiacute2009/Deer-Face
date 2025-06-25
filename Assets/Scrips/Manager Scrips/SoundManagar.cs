using UnityEngine;

public class SoundManagar : MonoBehaviour
{
    public static SoundManagar Instance { get; private set; }

    [Header("Gun Sounds")]
    public AudioSource shootingSoundAWM;
    public AudioSource reloadingSoundAWM;
    public AudioSource emptyManagizeSoundAWM;

    [Header("Movement Sounds")]
    public AudioSource move;
    public AudioSource jump;
    public AudioSource run;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // === Gun Sound Methods ===

    public void PlayShootingAWM()
    {
        if (!shootingSoundAWM.isPlaying)
            shootingSoundAWM.Play();
    }

    public void PlayReloadingAWM()
    {
        if (!reloadingSoundAWM.isPlaying)
            reloadingSoundAWM.Play();
    }

    public void PlayEmptyAWM()
    {
        if (!emptyManagizeSoundAWM.isPlaying)
            emptyManagizeSoundAWM.Play();
    }

    // === Movement Sound Methods ===

    public void PlayMove()
    {
        if (!move.isPlaying)
            move.Play();
    }

    public void StopMove()
    {
        if (move.isPlaying)
            move.Stop();
    }

    public void PlayRun()
    {
        if (!run.isPlaying)
            run.Play();
    }

    public void StopRun()
    {
        if (run.isPlaying)
            run.Stop();
    }

    public void PlayJump()
    {
        if (!jump.isPlaying)
            jump.Play();
    }
}
