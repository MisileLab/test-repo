using System.Collections;
using UnityEngine;

public class MaximItem : ItemAction
{
    public override string Id => "maxim";

    public override Vector2 Pos => new(39, 4);

    public override int Cost => 3;

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
        actionEnd = false;

        StartCoroutine(act());
    }

    IEnumerator act() {
        ActNpc.Comment("크흐흐흣흐");

        BookEvent @book = EventAction.Instance["book"] as BookEvent;

        foreach (Transform bk in @book.books) {
            bk.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        actionEnd = true;

        GameManager.Instance.health -= 6;
        @book.canClean = false;
        @book.Activated = false;
    }

    public override void OnUse(ref bool cancel)
    {
        if (!EventAction.Instance["book"].Activated && !(EventAction.Instance["book"] as BookEvent).canClean) {
            cancel = true;
        } else {
            ActNpc.Comment("아닛! 저것은..!!!");
        }
    }
}