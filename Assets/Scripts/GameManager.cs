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

    public bool isStarted, starting;
    public List<Npc> npcs = new();
    public int kicked = 0;
    public int health;
    void Start()
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

        if (action != null) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                highlight.SetActive(false);
                action = null;
            }
        }

        if (highlight.activeSelf) {
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
            Destroy(npc.GetComponent<Movement>());
            npc.Comment(kickComment[Random.Range(0, kickComment.Length)]);

            kicked++;

            yield return new WaitForSeconds(1);

            DestroyNpc(npc);
        }

        action = null;

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
