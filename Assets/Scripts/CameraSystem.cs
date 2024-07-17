using System;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    readonly Vector3 _movePerSecond = new(40, 40, 0); // z 0 고정
    Vector3 _pastMousePos;
    static readonly double Epsilon = 1.0e-10;
    
    public int limitScale = 25;
    public float camIntensity = 0.045f;
    public bool isIgnoringMove; // 위치 변경 무력화
    public bool isIgnoringCheck; // 위치 limit 체크 무력화
    public Vector2 limitCameraPosPositive = new(1, 1);
    public Vector2 limitCameraPosNegative = new(-1, -1);
    
    // ReSharper disable once PossibleNullReferenceException
    readonly Func<float, float, float> _getIntensity = (x, intensity) => intensity * (Camera.main.orthographicSize / 25) * Time.deltaTime * x;
    private readonly Func<float, float, bool> _accurateFloatCompare = (f, f2) => Math.Abs(f - f2) <= Epsilon;

    // Start is called before the first frame update
    void Start()
    {
        _pastMousePos = Input.mousePosition;
    }

    void MoveCamera(Func<float, float> callbackx, Func<float, float> callbacky) { // callback: float = amount, return move position
        if (Input.GetMouseButton(1) && Input.mousePosition != _pastMousePos) {
            Vector3 velocity = _pastMousePos-Input.mousePosition;
            if (!_accurateFloatCompare(Input.mousePosition.x, _pastMousePos.x)) { transform.Translate(callbackx(velocity.x), 0, 0); }
            if (!_accurateFloatCompare(Input.mousePosition.y, _pastMousePos.y)) { transform.Translate(0, callbacky(velocity.y), 0); }
        }
    }

    // ReSharper disable once PossibleNullReferenceException
    void Update() {
        if (!isIgnoringMove) {
            MoveCamera(
                x => _getIntensity(x, camIntensity) * _movePerSecond.x,
                y => _getIntensity(y, camIntensity) * _movePerSecond.y
            );
            _pastMousePos = Input.mousePosition;
            if (Input.mouseScrollDelta.y != 0) {
                Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
            }
        }
        if (!isIgnoringCheck) {
            // ReSharper disable once PossibleNullReferenceException
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
