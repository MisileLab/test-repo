using UnityEngine;

public class ClothEvent : EventAction
{
    public override string Id => "cloth";

    public override Vector2 Pos => new(36, 7);

    public override bool EndAction()
    {
        return false;
    }

    public override void StartAction()
    {
    }
}