using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Vector3 _startRot;
    [SerializeField] Vector3 _endRot;
    [SerializeField] float _duration = 2f;

    Coroutine _rotateCoroutine;

    private void Awake()
    {
        // 初期角度は反映せず、スクリプトから制御
        StartRotate();

        RotateCameraCoroutine();
    }

    /// <summary>
    /// 回転開始（中断にも対応）
    /// </summary>
    public void StartRotate()
    {
        if (_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
        }

        _rotateCoroutine = StartCoroutine(RotateCameraCoroutine());
    }

    /// <summary>
    /// カメラをstartRotからendRotへduration秒で回転
    /// </summary>
    private IEnumerator RotateCameraCoroutine()
    {
        Quaternion startQuat = Quaternion.Euler(_startRot);
        Quaternion endQuat = Quaternion.Euler(_endRot);

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            float t = elapsed / _duration;
            transform.rotation = Quaternion.Slerp(startQuat, endQuat, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endQuat;
    }
}
