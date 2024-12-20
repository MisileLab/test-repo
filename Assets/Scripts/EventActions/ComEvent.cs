using UnityEngine;
using System.Collections;

public class ComEvent : EventAction
{
    [SerializeField] GameObject blackPanel;
    public override string Id => "com";
    
    public bool actionEnd = false, canClean;
    public override bool StayActive => false;

    public override Vector2 Pos => new(47, 1);

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
        actionEnd = false;

        StartCoroutine(act());
    }
    IEnumerator act() {
        canClean = false;

        ActNpc.Comment("롤 해야지 ~");
        blackPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        canClean = true;

        GameManager.Instance.health += 10;
    }
}