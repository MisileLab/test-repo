using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGenerator : MonoBehaviour
{
    public static NpcGenerator Instance {get; private set;}
    public List<Npc> npcPrefabs = new();
    float time, delay;
    public Vector2 spawnFrom;
    public Vector2 spawnTo;
    
    void Start()
    {
        delay = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isStarted) {
            if (GameManager.Instance.npcs.Count < GameManager.Instance.maxNpc) {
                time += Time.deltaTime;

                if (time > delay) {
                    time = 0;

                    Npc npc = SpawnNpc(npcPrefabs[Random.Range(0, npcPrefabs.Count - 1)], spawnFrom);
                    npc.GetComponent<Movement>().dest = spawnTo;

                    delay = Random.Range(0.5f + npcPrefabs.Count * 0.5f, 2.5f + npcPrefabs.Count * 0.5f);
                }
            }
        }
    }

    public Npc SpawnNpc(Npc npc, Vector2 pos) {
        Npc spawned = Instantiate(npc, pos, Quaternion.identity);
        GameManager.Instance.npcs.Add(spawned);

        return spawned;
    }
}
