using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class EffectManager : SystemObject
{
    public static EffectManager instance { get; private set; } = null;

    /// <summary>
    /// �G�t�F�N�g�̃v���n�u
    /// </summary>
    [SerializeField]
    private List<GameObject> _effectOrigin = null;

    [SerializeField]
    private Canvas _UICanvas = null;

    public async override UniTask Initialize()
    {
        await base.Initialize();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _UICanvas.worldCamera = Camera.main;
    }

    public async UniTask CreateEffect(int index, Vector3 position, Quaternion rotation)
    {
        // �G�t�F�N�g�̃v���n�u���擾
        if (!IsEnableIndex(_effectOrigin, index))
        {
            return;
        }
        // �G�t�F�N�g�𐶐�
        GameObject effect = Instantiate(_effectOrigin[index], position, rotation);
        effect.transform.SetParent(_UICanvas.transform);
        // �G�t�F�N�g�̍Đ�
        var particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        // �G�t�F�N�g�̏I����ҋ@
        await UniTask.WaitUntil(() => !particleSystem.isPlaying);
        // �G�t�F�N�g��j��
        Destroy(effect);
    }

    public async UniTask CreateScreenEffect(int index, Vector3 position, Quaternion rotation)
    {
        // �G�t�F�N�g�̃v���n�u���擾
        if (!IsEnableIndex(_effectOrigin, index))
        {
            return;
        }
        Camera camera = Camera.main;
        Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(position.x, position.y, camera.nearClipPlane + 0.5f));
        // �G�t�F�N�g�𐶐�
        GameObject effect = Instantiate(_effectOrigin[index], worldPos, rotation);
        // �G�t�F�N�g�̍Đ�
        var particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        // �G�t�F�N�g�̏I����ҋ@
        await UniTask.WaitUntil(() => !particleSystem.isPlaying);
        // �G�t�F�N�g��j��
        Destroy(effect);
    }
}
