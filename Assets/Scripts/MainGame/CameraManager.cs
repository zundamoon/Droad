using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : SystemObject
{
    public static CameraManager instance;

    public float dragSpeed = 0.1f;
    public float zoomSpeed = 10f;
    public float zoomMin = 5f;
    public float zoomMax = 100f;
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
        if(Input.GetMouseButton(0)) return;

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * Time.deltaTime;

            _camera.transform.Translate(move, Space.Self);

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
