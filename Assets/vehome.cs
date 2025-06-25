using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class vehome : MonoBehaviour
{
    [SerializeField] private Button _btnBack;
    [SerializeField] private GameObject _load;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _load.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBack()
    {
        _load?.SetActive(true);
        new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("home1");
        


    }
}
