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
        // �����p�x�͔��f�����A�X�N���v�g���琧��
        StartRotate();

        RotateCameraCoroutine();
    }

    /// <summary>
    /// ��]�J�n�i���f�ɂ��Ή��j
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
    /// �J������startRot����endRot��duration�b�ŉ�]
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
