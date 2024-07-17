using System.Collections;
using UnityEngine;

public class BatItem : ItemAction
{
    public override string Id => "bat";

    public override Vector2 Pos => new();

    public override int Cost => 5;

    bool actionEnd;
    public override bool EndAction()
    {
        if (Activated) {
            return actionEnd;
        } else {
            return false;
        }
    }

    public override void StartAction()
    {
        bool b = false;
        OnUse(ref b);
    }

    public override void OnUse(ref bool cancel)
    {
        cancel = true;

        GameManager.Instance.cost -= 5;

        ActNpc.Comment("으악..!");
        var movement = ActNpc.GetComponent<Movement>();

        movement.EventEnd();
        movement.ItemEnd();
        movement.state = 1;
        movement.delay = -1;
    }
}