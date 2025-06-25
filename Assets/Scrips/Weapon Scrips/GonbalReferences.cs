using UnityEngine;

public class GonbalReferences : MonoBehaviour
{
   public static GonbalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefab;
    public GameObject bulletImpactEffectPrefab_Default;
    public GameObject bulletImpactEffectPrefab_Deer;
    public GameObject bulletImpactEffectPrefab_Rock;
    public GameObject bulletImpactEffectPrefab_Wood;
    public GameObject bulletImpactEffectPrefab_Weater;

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
}
