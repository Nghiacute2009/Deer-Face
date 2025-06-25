using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScopeController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public RawImage scopeOverlay;
    public GameObject gunModel;

    [Header("Zoom Settings")]
    public float zoomFOV = 30f;
    public float minFOV = 15f;
    public float maxFOV = 40f;
    public float zoomSpeed = 8f;

    [Header("Breath & Recoil Settings")]
    public float breathIntensity = 0.05f;
    public float breathSpeed = 1.5f;
    public float recoilAmount = 0.05f;
    public float recoilSpeed = 10f;

    private float defaultFOV;
    private bool isScoped = false;
    private Vector3 originalCamLocalPos;
    private Coroutine recoilRoutine;

    void Start()
    {
        defaultFOV = playerCamera.fieldOfView;
        originalCamLocalPos = playerCamera.transform.localPosition;

        if (scopeOverlay != null) scopeOverlay.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ToggleScope();

        if (isScoped)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zoomFOV -= scroll * 10f;
            zoomFOV = Mathf.Clamp(zoomFOV, minFOV, maxFOV);

            // Breath effect
            float breathOffset = Mathf.Sin(Time.time * breathSpeed) * breathIntensity;
            playerCamera.transform.localPosition = new Vector3(0, breathOffset, 0);
        }
        else
        {
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                originalCamLocalPos,
                Time.deltaTime * 10f);
        }

        float targetFOV = isScoped ? zoomFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void ToggleScope()
    {
        isScoped = !isScoped;
        if (scopeOverlay != null) scopeOverlay.enabled = isScoped;
        if (gunModel != null) gunModel.SetActive(!isScoped);
    }

    public bool IsScoped() => isScoped;

    public void TriggerScopeRecoil()
    {
        if (recoilRoutine != null)
            StopCoroutine(recoilRoutine);
        recoilRoutine = StartCoroutine(ScopeRecoil());
    }

    private IEnumerator ScopeRecoil()
    {
        Vector3 back = originalCamLocalPos + new Vector3(0, 0, -recoilAmount);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * recoilSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(originalCamLocalPos, back, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * recoilSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(back, originalCamLocalPos, t);
            yield return null;
        }
    }
}