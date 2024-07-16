using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGenerator : MonoBehaviour
{
    public static NpcGenerator Instance {get; private set;}
    public List<Npc> npcPrefabs = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Npc SpawnNpc(Npc npc, Vector2 pos) {
        Npc spawned = Instantiate(npc, pos, Quaternion.identity);
        GameManager.Instance.npcs.Add(spawned);

        return spawned;
    }
}
