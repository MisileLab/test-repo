using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public float gameTime = 0;
    public string[] kickComment = {};
    public string action = null;
    public GameObject highlight;

    public bool isStarted, starting, highlighting;
    public List<Npc> npcs = new();
    public int kicked = 0;
    public int health;

    public List<EventAction> events;

    public List<bool> isActive;//0-cloth,1-book,2-com,3-bed,4-console
    void Awake()
    {
        Instance = this;
        GameStart();
    }

    public void GameStart() {
        StartCoroutine(gmStart());
    }

    IEnumerator gmStart() {
        isStarted = false;
        starting = true;

        kicked = 0;
        health = 5;

        for (int i = 0; i < 3; i++) {
            Debug.Log(3 - i);

            yield return new WaitForSeconds(1);
        }

        isStarted = true;
        starting = false;
    }

    void Update() {
        if (isStarted) {
            gameTime += Time.deltaTime;
        }

        if (health < 5) health = 5;

        if (action != null) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                highlight.SetActive(false);
                action = null;
            }
        }

        highlighting = highlight.activeSelf;

        if (highlighting) {
            Time.timeScale = 0.3f;
        } else {
            Time.timeScale = 1;
        }
    }

    public void OnSelect(Npc npc) {
        StartCoroutine(selectAction(npc));
    }

    IEnumerator selectAction(Npc npc) {
        highlight.SetActive(false);

        if (action == "kick") {
            action = null;
            var movement = npc.GetComponent<Movement>();
            movement.EventEnd();
            Destroy(movement);
            
            npc.Comment(kickComment[Random.Range(0, kickComment.Length)]);

            kicked++;

            yield return new WaitForSeconds(1);

            DestroyNpc(npc);
        } else {
            action = null;
        }

        yield return null;
    }

    public void DestroyNpc(Npc npc) {
        npcs.Remove(npc);
        Destroy(npc.gameObject);
    }

    public void Kick() {
        if (kicked >= 5) return;

        highlight.SetActive(true);
        action = "kick";
    }
}
