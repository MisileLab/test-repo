using UnityEngine;
using UnityEngine.SceneManagement;

public class Ui : MonoBehaviour
{
    public GameObject panel;
    void Awake(){
        panel.SetActive(false);
    }
    void Update(){
        if(GameManager.Instance.health>=100){
            panel.SetActive(true);
        }
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
