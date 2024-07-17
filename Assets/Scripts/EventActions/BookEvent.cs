using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookEvent : EventAction
{
    [SerializeField] GameObject book_Set;
    [SerializeField] List<string> comments = new();
    public List<Transform> books = new List<Transform>();
    public bool actionEnd = false, canClean;
    public override string Id => "book";
    public override bool StayActive => true;

    Vector2 center = new(0.882f, -0.065f);

    void Awake() {
        foreach (Transform book in book_Set.transform) {
            books.Add(book);

            book.gameObject.SetActive(false);
        }
    }

    public override Vector2 Pos => new(39, 4);

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

        ActNpc.Comment(comments[Random.Range(0, comments.Count - 1)]);
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
        canClean = true;

        GameManager.Instance.health += 10;
    }
}