using System.Collections;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] Sprite front, back;
    public Animator animator;
    public SpriteRenderer render;
    public int moveState = 0;
    public bool isFront = false;
    float moveTime;
    IEnumerator moveRoutine;
    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        //MoveTo(transform.position + new Vector3(0, 3, 0), 1); //test
    }

    void Update()
    {
        animator.SetInteger("moveState", moveState);

        if (moveState != 0) {
            moveTime += Time.deltaTime;

            if (moveTime > 0.2f) {
                moveState = 0;
                moveTime = 0;
            }
        }

        if (moveState == 0) {
            if (isFront) {
                render.sprite = front;
            } else {
                render.sprite = back;
            }
        }
    }

    public void Move(Vector3 vel) {
        moveTime = 0;

        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y)) {
            if (vel.x > 0) {
                moveState = 1;
                render.flipX = false;
            } else if (vel.x < 0) {
                moveState = 1;
                render.flipX = true;
            }
        } else {
            if (vel.y >= 0.1) {
                moveState = 2;
                isFront = false;
            } else {
                moveState = 3;
                isFront = true;
            }
        }

        transform.Translate(vel);
    }

    public void MoveTo(Vector3 pos, float sec) {
        if (moveRoutine != null) {
            StopCoroutine(moveRoutine);
        }
        moveRoutine = moveTo(pos, sec);

        StartCoroutine(moveRoutine);
    }

    IEnumerator moveTo(Vector3 pos, float sec) {
        Vector3 differ = pos - transform.position;
        for (int i = 0; i <= 20; i++) {
            Move(differ / 20);

            yield return new WaitForSeconds(sec / 20);
        }

        moveRoutine = null;
    }
}
