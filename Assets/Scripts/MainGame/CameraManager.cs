using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : SystemObject
{
    public static CameraManager instance;

    private float dragSpeed = 0.5f;
    private float zoomSpeed = 50f;
    private float zoomMin = 5.0f;
    private float zoomMax = 30.0f;
    private float moveLimitRange = 30.0f;

    private Transform zoomTarget;

    private static Camera _camera = null;

    private Vector3 lastMousePos;

    private const float _CHANGE_TARGET_TIME = 1.0f;

    public override async UniTask Initialize()
    {
        instance = this;
        _camera = Camera.main;
        zoomTarget = StageManager.instance.stagePrefab.transform;
    }

    public static async UniTask SetAnchor(Transform anchorTransform, float moveTime = _CHANGE_TARGET_TIME)
    {
        Vector3 oldPosition = _camera.transform.position;
        Vector3 oldRotation = _camera.transform.eulerAngles;
        float elapsedTime = 0;
        while (elapsedTime < _CHANGE_TARGET_TIME)
        {
            elapsedTime += Time.deltaTime;
            float ratio = Mathf.Clamp01(elapsedTime / _CHANGE_TARGET_TIME);
            float smooth = Mathf.SmoothStep(0, 1, ratio);
            _camera.transform.position = Vector3.Lerp(oldPosition, anchorTransform.position, smooth);
            _camera.transform.eulerAngles = Vector3.Lerp(oldRotation, anchorTransform.eulerAngles, smooth);
            await UniTask.DelayFrame(1);
        }
        _camera.transform.SetParent(anchorTransform);
    }

    public async UniTask CameraDrag()
    {
        if (Input.GetMouseButton(0)) return;

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;

            // カメラの距離に応じてドラッグ移動速度をスケール調整
            float distance = Vector3.Distance(_camera.transform.position, zoomTarget.position);
            float scaledDragSpeed = dragSpeed * (distance / 10f);  // ← 数値調整可

            Vector3 right = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;

            Vector3 move = (-right * delta.x + -forward * delta.y) * scaledDragSpeed * Time.deltaTime;

            Vector3 newPosition = _camera.transform.position + move;

            // 「zoomTargetを中心」に範囲制限する
            Vector3 center = zoomTarget.position;
            Vector3 offset = newPosition - center;

            offset = Vector3.ClampMagnitude(offset, moveLimitRange);
            newPosition = center + offset;

            // Y座標は固定
            newPosition.y = _camera.transform.position.y;

            _camera.transform.position = newPosition;

            lastMousePos = Input.mousePosition;
        }
    }

    public async UniTask CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // カメラの forward に沿ってズーム
            Vector3 direction = _camera.transform.forward;

            // 新しい位置を仮計算
            Vector3 newPosition = _camera.transform.position + direction * scroll * zoomSpeed;

            // 中心からの距離を計算
            float distance = Vector3.Distance(newPosition, zoomTarget.position);

            // 距離制限内であれば移動を適用
            if (distance >= zoomMin && distance <= zoomMax)
            {
                _camera.transform.position = newPosition;
            }
        }
    }
}
