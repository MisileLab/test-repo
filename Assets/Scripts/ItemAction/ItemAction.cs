using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAction : MonoBehaviour
{
    public abstract string Id { get; }
    public abstract Vector2 Pos { get; }
    public abstract int Cost { get; }

    public bool Activated = false;
    public Npc ActNpc = null;

    void Start() {
        GameManager.Instance.items.Add(this);
    }

    public void Use() {
        if (Activated || GameManager.Instance.cost < Cost) return;

        GameManager.Instance.action = "using";
        GameManager.Instance.highlight.SetActive(true);
        GameManager.Instance.item = this;
    }

    public abstract void OnUse(ref bool cancel);
    public abstract void StartAction();
    public abstract bool EndAction();
    public virtual void InAction(){}
}
