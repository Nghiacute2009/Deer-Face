using UnityEngine;

public class ControllerButterR : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public NhiemVu NhiemVu;
    // [SerializeField] public Animator animator;

    // Update is called once per frame
    void Start()
    {
        if (NhiemVu == null)
        {
            NhiemVu = FindObjectOfType<NhiemVu>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullte"))
        {
            NhiemVu.GietNaiFake();
            Debug.Log("Da giet 1 con nai fake ");
            Destroy(gameObject, 0.5f);
        }
    }
}
