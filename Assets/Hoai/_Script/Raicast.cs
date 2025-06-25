using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Raicast : MonoBehaviour
{
    [SerializeField]
    public LayerMask layerTree; // Layer mà mình muốn va chạm
    [SerializeField] Slider healthSlider; // Slider để hiển thị máu
    [SerializeField] TextMeshProUGUI healthText; // Text để hiển thị giá trị máu
    [SerializeField] private float health = 0f; // Giá trị máu ban đầu
    private const float maxHealth = 100f; // Giá trị máu tối đa
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cập nhật giá trị máu và hiển thị trên UI
        // healthSlider.value = health;
        healthText.text = health.ToString("F0"); // Hiển thị giá trị máu dưới dạng số nguyên 



        if (Physics.Raycast(transform.position, transform.forward, out var hit, 5, layerTree))
        {
            //va chạm tới layer mà mình mong muốn
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            Debug.Log(hit.transform.name);
            Destroy(hit.transform.gameObject);
            health += 1f; // Tăng máu khi va chạm với đối tượng
            if (health > maxHealth)
            {
                health = maxHealth; // Giới hạn máu không vượt quá giá trị tối đa
            }
            // healthSlider.value = health; // Cập nhật giá trị máu trên slider
            healthText.text = health.ToString("F0"); // Cập nhật giá trị máu trên text

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);
        }
    }
}
