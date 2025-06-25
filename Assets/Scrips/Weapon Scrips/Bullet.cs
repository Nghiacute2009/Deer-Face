using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        string tag = hitObject.tag;

        Debug.Log("Hit: " + tag);

        // Tạo hiệu ứng va chạm
        CreateImpactEffect(collision, tag);

        // Nếu trúng bia "Target", gọi xử lý respawn
        if (tag == "Target")
        {
            TargetRespawn target = hitObject.GetComponent<TargetRespawn>();
            if (target != null)
            {
                target.OnHit();
            }
        }

        // Huỷ viên đạn sau khi va chạm
        Destroy(gameObject);
    }

    void CreateImpactEffect(Collision collision, string tag)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject effectPrefab = GetEffectByTag(tag);

        if (effectPrefab != null)
        {
            GameObject impact = Instantiate(
                effectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );
            impact.transform.SetParent(collision.gameObject.transform);
        }
    }

    GameObject GetEffectByTag(string tag)
    {
        switch (tag)
        {
            case "Deer":
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Deer;
            case "Enemy":
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Weater;
            case "Rock":
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Rock;
            case "Wood":
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Wood;
            case "Wall":
                return GonbalReferences.Instance.bulletImpactEffectPrefab;
            case "Target":
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Wood; // hoặc hiệu ứng riêng cho bia
            default:
                return GonbalReferences.Instance.bulletImpactEffectPrefab_Default;
        }
    }
}
