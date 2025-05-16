using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    private static Camera _camera = null;

    private const float _CHANGE_TARGET_TIME = 1.0f;

    public static void Init()
    {
        _camera = Camera.main;
    }

    public static async UniTask SetAnchor(Transform anchorTransform , float moveTime = _CHANGE_TARGET_TIME)
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
}
