using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothEvent : EventAction
{
    [SerializeField] GameObject clothes_set;
    [SerializeField] List<Transform> clothes = new List<Transform>();
    public bool actionEnd = false;

    public override string Id => "cloth";

    public override Vector2 Pos => new(36, 7);
    Vector2 center = new(37.87f, 13.9f);

    void Awake() {
        foreach (Transform clothe in clothes_set.transform) {
            clothes.Add(clothe);

            clothe.gameObject.SetActive(false);
        }
    }

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
        ActNpc.Comment("입을 옷이 없잖아ㅏ!");
        foreach (Transform clothe in clothes) {
            clothe.gameObject.SetActive(false);
        }

        Vector2 pos;

        foreach (Transform clothe in clothes) {
            clothe.gameObject.SetActive(true);
            pos = clothe.position;

            clothe.position = center;

            yield return new WaitForSeconds(0.05f);

            LeanTween.move(clothe.gameObject, pos, 0.3f);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);

        actionEnd = true;

        GameManager.Instance.health += 10;
    }
}