using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookEvent : EventAction
{
    [SerializeField] GameObject book_Set;
    [SerializeField] List<Transform> books = new List<Transform>();
    public bool actionEnd = false;
    public override string Id => "book";

    Vector2 center = new(0.882f, -0.065f);

    void Awake() {
        foreach (Transform book in book_Set.transform) {
            books.Add(book);

            book.gameObject.SetActive(false);
        }
    }

    public override Vector2 Pos => new(40, 5);

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
        ActNpc.Comment("아이코 실수 ~");
        foreach (Transform book in books) {
            book.gameObject.SetActive(false);
        }

        Vector2 pos;

        foreach (Transform book in books) {
            book.gameObject.SetActive(true);
            pos = book.position;

            book.localPosition = center;

            yield return new WaitForSeconds(0.05f);

            LeanTween.move(book.gameObject, pos, 0.3f);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);

        actionEnd = true;
    }
}