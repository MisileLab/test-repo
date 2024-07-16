using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text timerText;
    public Text timerText_back;
    public Text npcCount;
    public Slider HPbar;
    public Text HPtext;
    public Text HPtext_back;
    public Text kick_amount;
    float timer;

    void Update() {
        timer = GameManager.Instance.gameTime;

        timerText.text = "Time " + ((timer / 60 < 10) ? "0" + ((int)(timer / 60)) : ((int)(timer / 60)).ToString()) + ":" + ((timer % 60 < 10) ? "0" + ((int)timer % 60) : ((int)timer % 60).ToString());

        timerText_back.text = timerText.text;

        npcCount.text = "활동중인 인원: " + GameManager.Instance.npcs.Count.ToString();

        HPbar.value = (float)GameManager.Instance.health / 100;

        HPtext_back.text = HPtext.text = (100 - GameManager.Instance.health).ToString() + "%";

        kick_amount.text = "쫓겨난 사람: " + GameManager.Instance.kicked.ToString() + "/5";
    }
}