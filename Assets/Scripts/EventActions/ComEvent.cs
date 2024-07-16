using UnityEngine;

public class ComEvent : EventAction
{
    public override string Id => "com";

    public override Vector2 Pos => new(45, 3);

    public override bool EndAction()
    {
        return false;
    }

    public override void StartAction()
    {
    }
}