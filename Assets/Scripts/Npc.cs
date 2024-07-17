using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    [SerializeField] Sprite front, back;
    public Animator animator;
    public SpriteRenderer render;
    public int moveState = 0;
    public bool isFront = false;
    float moveTime;
    IEnumerator moveRoutine;
    [SerializeField] Image balloon;
    [SerializeField] Text comment;
    public Text info;
    float balTime = 0;

    [SerializeField] Material outline;
    Material defMat;

    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        defMat = render.material;

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

        if (comment.text != "") {
            balTime += Time.deltaTime;

            if (balTime > 2) {
                Color col = comment.color;
                col.a = 3.5f - balTime;

                Color col2 = balloon.color;
                col2.a = 3.5f - balTime;

                comment.color = col;
                balloon.color = col2;

                if (balTime > 3.5f) {
                    comment.text = "";
                    balTime = 0;

                    balloon.gameObject.SetActive(false);
                }
            }
        }

        MouseMapping();
    }

    public void Comment(string str) {
        balTime = 0;
        comment.text = str;

        Color col = comment.color;
        col.a = 1;

        comment.color = col;

        Color col2 = balloon.color;
        col2.a = 1;

        balloon.color = col2;

        balloon.gameObject.SetActive(true);
    }

    void MouseMapping() {

        Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        if (Vector3.Distance(new Vector3(mPos.x, mPos.y), new Vector3(transform.position.x, transform.position.y)) <= 2) {
            if (GameManager.Instance.highlighting)
                render.material = outline;
            else
                render.material = defMat;

            
            if (Input.GetMouseButtonDown(0)) {
                OnClick();
            }
        } else {
            render.material = defMat;
        }
    }

    public void OnClick() {
        GameManager.Instance.OnSelect(this);
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
            if (vel.y >= 0) {
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
