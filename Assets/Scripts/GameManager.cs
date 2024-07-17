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
    public int kicked = 0, kickMax, maxNpc;
    public int health;
    public float cost;

    public List<EventAction> events;
    public List<ItemAction> items;
    public ItemAction item = null;

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
            cost += Time.deltaTime * 0.5f;

            kickMax = 5 + (int)(gameTime / 60);
            maxNpc = 6 + (int)(gameTime / 60);
            //maxNpc = 1;
        }

        if (health < 5) health = 5;
        if (cost > 10) cost = 10;

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
        } if (action == "using") {
            if (item != null && cost >= item.Cost) {
                bool cancel = false;

                item.ActNpc = npc;
                item.OnUse(ref cancel);

                if (cancel) {
                    item.ActNpc = null;
                } else {
                    cost -= item.Cost;
                    npc.GetComponent<Movement>().StartItem(item);
                }

                item = null;
            }

            action = null;
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
        if (kicked >= kickMax) return;

        highlight.SetActive(true);
        action = "kick";
    }
}
