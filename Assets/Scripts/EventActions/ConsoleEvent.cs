using UnityEngine;

public class ConsoleEvent : EventAction
{
    public override string Id => "console";

    public override Vector2 Pos => new(21, 4);

    public override bool EndAction()
    {
        return false;
    }

    public override void StartAction()
    {
    }
}