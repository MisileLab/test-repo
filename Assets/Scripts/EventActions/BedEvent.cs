using UnityEngine;

public class BedEvent : EventAction
{
    public override string Id => "bed";

    public override Vector2 Pos => new(57, -4);

    public override bool EndAction()
    {
        return false;
    }

    public override void StartAction()
    {
    }
}