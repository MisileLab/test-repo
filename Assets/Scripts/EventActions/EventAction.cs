using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventAction : MonoBehaviour
{
    public abstract string Id { get; }
    public abstract Vector2 Pos { get; }

    public bool Activated = false;
    public Npc ActNpc = null;

    void Start() {
        GameManager.Instance.events.Add(this);
    }

    public abstract void StartAction();
    public abstract bool EndAction();
    public virtual void InAction(){}
}
