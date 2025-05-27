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

    private static Camera _camera = null;

    private Vector3 lastMousePos;

    private const float _CHANGE_TARGET_TIME = 1.0f;

    public override async UniTask Initialize()
    {
        instance = this;
        _camera = Camera.main;
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

    public void CameraDrag()
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


    public void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 direction = _camera.transform.forward;
            _camera.transform.position += direction * scroll * zoomSpeed;

            float distance = Vector3.Distance(_camera.transform.position, Vector3.zero);
            if (distance < zoomMin) _camera.transform.position = Vector3.zero + direction * zoomMin;
            else if (distance > zoomMax) _camera.transform.position = Vector3.zero + direction * zoomMax;
        }
    }
}
