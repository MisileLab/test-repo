using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Vector3 movePerSecond = new(40, 40, 0); // z 0 고정
    Vector3 pastMousePos;
    public int limitScale = 25;
    public float camIntensity = 0.045f;
    public bool isIgnoringMove = false; // 위치 변경 무력화
    public bool isIgnoringCheck = false; // 위치 limit 체크 무력화
    public Vector2 limitCameraPosPositive = new(1, 1);
    public Vector2 limitCameraPosNegative = new(-1, -1);

    // Start is called before the first frame update
    void Start()
    {
        pastMousePos = Input.mousePosition;
    }

    void Update() {
        var velocity = pastMousePos-Input.mousePosition;
        if (!isIgnoringMove) {
            if (Input.GetMouseButton(0) && Input.mousePosition != pastMousePos) {
                if (Input.mousePosition.x != pastMousePos.x) {
                    transform.Translate(movePerSecond.x * Time.deltaTime * velocity.x * camIntensity * (Camera.main.orthographicSize / 25), 0, 0);
                }
                if (Input.mousePosition.y != pastMousePos.y) {
                    transform.Translate(0, movePerSecond.y * Time.deltaTime * velocity.y * camIntensity * (Camera.main.orthographicSize / 25), 0);
                }
            }
            pastMousePos = Input.mousePosition;
            if (Input.mouseScrollDelta.y != 0) {
                Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
            }
        }
        if (!isIgnoringCheck) {
            if (Camera.main.orthographicSize <= 0.1F) {
                Camera.main.orthographicSize = 0.1F;
            } else if (Camera.main.orthographicSize > limitScale) {
                Camera.main.orthographicSize = limitScale;
            }
            var finalPos = transform.position;
            if (limitCameraPosPositive.x < finalPos.x) {
                finalPos.x = limitCameraPosPositive.x;
            } else if (limitCameraPosNegative.x > finalPos.x) {
                finalPos.x = limitCameraPosNegative.x;
            }
            if (limitCameraPosPositive.y < finalPos.y) {
                finalPos.y = limitCameraPosPositive.y;
            } else if (limitCameraPosNegative.y > finalPos.y) {
                finalPos.y = limitCameraPosNegative.y;
            }
            transform.position = finalPos;
        }
    }
}
