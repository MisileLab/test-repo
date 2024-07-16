using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text timerText;
    public Text timerText_back;
    public Text npcCount;
    float timer;

    void Update() {
        timer = GameManager.Instance.gameTime;

        timerText.text = "Time " + ((timer / 60 < 10) ? "0" + ((int)(timer / 60)) : ((int)(timer / 60)).ToString()) + ":" + ((timer % 60 < 10) ? "0" + ((int)timer % 60) : ((int)timer % 60).ToString());

        timerText_back.text = timerText.text;

        npcCount.text = "활동중인 사람: " + GameManager.Instance.npcs.Count.ToString();
    }
}