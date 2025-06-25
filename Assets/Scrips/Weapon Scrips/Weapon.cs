using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera; // Camera chính của người chơi
    public ScopeEffectHandler scopeEffectHandler; // Script xử lý hiệu ứng scope (zoom, thở, giật)
    private Animator animator; // Animator của vũ khí

    [Header("Shooting")]
    public bool isShooting, readyToShoot; // Trạng thái đang bắn và sẵn sàng bắn
    bool allowReset = true;
    public float shootingDelay = 0.2f; // Độ trễ giữa mỗi lần bắn

    public int bulletsPerBurst = 3; // Số viên đạn mỗi lần bắn burst
    public int burstBulletsLeft;   // Số viên còn lại trong lần bắn burst
    public float spreadIntensity = 0.01f; // Độ lệch đạn (spread)
    public GameObject bulletPrefab; // Prefab viên đạn
    public Transform bulletSpwan;   // Vị trí xuất hiện viên đạn
    public float bulletVelocity = 100f; // Tốc độ bay đạn
    public float bulletPrefabLifeTime = 3f; // Thời gian tồn tại viên đạn
    public GameObject muzzleEffect; // Hiệu ứng lửa đầu nòng

    [Header("Reloading")]
    public float reloadTime = 3f;
    public int magazineSize = 5, bulletsLeft; // Dung lượng và số đạn còn lại
    public bool isReloading;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode; // Chế độ bắn

    private bool canShoot = true; // Có thể bắn hay không

    [Header("Aiming Settings")]
    public bool isAiming;     // Trạng thái đang ngắm
    public bool isScoped = false; // Trạng thái bật scope (ống nhắm)
    public float aimFOV = 30f;    // Góc nhìn khi scope
    private float normalFOV;      // Góc nhìn ban đầu (không scope)
    public float aimSpeed = 8f;   // Tốc độ chuyển giữa FOV
    public float minFOV = 15f, maxFOV = 40f;

    [Header("Scope UI")]
    public RawImage scopeOverlay; // UI ống ngắm overlay
    public GameObject gunModel;   // Hiển thị hoặc ẩn khẩu súng khi scope

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        normalFOV = playerCamera.fieldOfView;

        // Ẩn scope overlay và bật súng khi bắt đầu
        if (scopeOverlay != null)
            scopeOverlay.gameObject.SetActive(false);

        if (gunModel != null)
            gunModel.SetActive(true);
    }

    void Update()
    {
        if (!canShoot || isReloading) return;

        // Ngăn bắn khi trỏ vào UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        // Cập nhật UI đạn
        if (AmmoManager.Instance.amnoDisplay != null)
            AmmoManager.Instance.amnoDisplay.text = $"{bulletsLeft}/{magazineSize}";

        // Nạp đạn nếu nhấn R
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize)
        {
            Reload();
            return;
        }

        // Nhấn chuột phải để bật/tắt scope
        if (Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            isAiming = isScoped;

            if (animator != null)
                animator.SetBool("IsAiming", isScoped);

            if (scopeOverlay != null)
                scopeOverlay.gameObject.SetActive(isScoped);

            if (gunModel != null)
                gunModel.SetActive(!isScoped);
        }

        // Cuộn chuột để điều chỉnh FOV khi scope
        if (isScoped)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            aimFOV -= scroll * 10f;
            aimFOV = Mathf.Clamp(aimFOV, minFOV, maxFOV);
        }

        // Zoom FOV mượt mà
        float targetFOV = isScoped ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);

        // Áp dụng hiệu ứng thở khi scope
        if (scopeEffectHandler != null)
            scopeEffectHandler.ApplyBreathing(isScoped);

        // Xác định trạng thái bắn theo chế độ
        switch (currentShootingMode)
        {
            case ShootingMode.Auto:
                isShooting = Input.GetKey(KeyCode.Mouse0);
                break;
            case ShootingMode.Single:
            case ShootingMode.Burst:
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
                break;
        }

        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            if (currentShootingMode == ShootingMode.Burst)
                burstBulletsLeft = bulletsPerBurst;

            FireWeapon();
        }

        // Hết đạn nhưng vẫn giữ chuột bắn → phát âm thanh empty
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManagar.Instance?.emptyManagizeSoundAWM?.Play();
        }
    }

    // Xử lý bắn
    private void FireWeapon()
    {
        if (!readyToShoot || isReloading) return;

        readyToShoot = false;
        allowReset = false;
        bulletsLeft--;

        if (muzzleEffect != null)
            muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (animator != null)
            animator.SetTrigger("Recoil");

        SoundManagar.Instance?.shootingSoundAWM?.Play();

        // Tạo viên đạn và bắn
        Vector3 shootingDirection = CalculateDirecttionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpwan.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = shootingDirection * bulletVelocity;

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        Invoke(nameof(ResetShot), shootingDelay);

        // Bắn burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }

        // Gọi giật camera nếu đang scope
        if (isScoped && scopeEffectHandler != null)
            scopeEffectHandler.TriggerRecoil();
    }

    // Nạp đạn
    private void Reload()
    {
        if (isReloading || bulletsLeft == magazineSize) return;

        isReloading = true;

        // Nếu đang scope thì tắt scope trước khi nạp đạn
        if (isScoped)
        {
            playerCamera.fieldOfView = normalFOV;

            isScoped = false;
            isAiming = false;

            if (animator != null)
                animator.SetBool("IsAiming", false);

            if (scopeOverlay != null)
                scopeOverlay.gameObject.SetActive(false);

            if (gunModel != null)
                gunModel.SetActive(true);
        }

        SoundManagar.Instance?.reloadingSoundAWM?.Play();

        if (animator != null)
            animator.SetTrigger("Reload");

        Invoke(nameof(ReloadCompleted), reloadTime);
    }


    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    // Cho phép bắn lại sau delay
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    // Tính toán hướng bắn + độ lệch (spread)
    private Vector3 CalculateDirecttionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(100);
        Vector3 direction = targetPoint - bulletSpwan.position;

        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    // Tự hủy viên đạn sau thời gian
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    // Cho phép bật/tắt khả năng bắn
    public void EnableShooting(bool enable)
    {
        canShoot = enable;
    }

    // Cập nhật trạng thái chạy (ảnh hưởng tới animation vũ khí)
    public void SetRunningState(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", isRunning);
        }
    }
}
