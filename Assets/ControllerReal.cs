using UnityEngine;

public class ControllerButter : MonoBehaviour
{
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
            NhiemVu.GietNaiReal();
            NhiemVu.TruThoiGian();
            Debug.Log("Da giet 1 con nai THIET ");
            Destroy(gameObject,0.5f);
        }
    }
}
