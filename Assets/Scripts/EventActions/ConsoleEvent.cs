using UnityEngine;
using System.Collections;

public class ConsoleEvent : EventAction
{
    [SerializeField] GameObject blackPanel;
    public override string Id => "console";
    public bool actionEnd = false, canClean;
    public override bool StayActive => false;

    public override Vector2 Pos => new(22, -5);

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

        ActNpc.Comment("아이코 실수 ~");
        blackPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        canClean = true;

        GameManager.Instance.health += 10;
    }
}