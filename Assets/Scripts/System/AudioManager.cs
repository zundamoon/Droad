using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;
using static GameEnum;

public class AudioManager : SystemObject
{
    [SerializeField]
    private GameObject _bgmSourceOrigin;

    [SerializeField]
    private GameObject _seSourceOrigin;

    [SerializeField]
    private BGMClip _bgmClip;

    [SerializeField]
    private SEClip _seClip;

    public static AudioManager instance = null;

    private List<AudioSource> BGMSourceList;
    private List<AudioSource> SESourceList;

    public override async UniTask Initialize()
    {
        instance = this;

        BGMSourceList = new List<AudioSource>();
        SESourceList = new List<AudioSource>();
    }

    /// <summary>
    /// BGM�𗬂��֐�
    /// </summary>
    /// <param name="num"></param>
    public void PlayBGM(BGM num)
    {
        AudioSource source = GetUnusedBGMSource(BGMSourceList);
        source.clip = _bgmClip.bgmClips[(int)num];
        source.Play();
    }

    /// <summary>
    /// BGM���~�߂�֐�
    /// </summary>
    /// <param name="num"></param>
    public void StopBGM()
    {
        AudioSource source = BGMSourceList[0];
        source.Stop();
    }

    /// <summary>
    /// SE��炷�֐�
    /// </summary>
    /// <param name="num"></param>
    public void PlaySE(SE num)
    {
        AudioSource source = GetUnusedSESource(SESourceList);
        source.PlayOneShot(_seClip.seClips[(int)num]);
    }

    // �����̃\�[�X�𗘗p���čĐ�����
    public void PlaySE(SE num, AudioSource source)
    {
        source.PlayOneShot(_seClip.seClips[(int)num]);
    }

    /// <summary>
    /// ���g�p�̃\�[�X���擾����
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private AudioSource GetUnusedBGMSource(List<AudioSource> source)
    {
        int number = -1;
        for (int i = 0, max = source.Count; i < max; i++)
        {
            if (source[i].time > 0) continue;

            number = i;
            break;
        }
        if (number == -1)
        {
            source.Add(Instantiate(_bgmSourceOrigin, transform).GetComponent<AudioSource>());
            number = source.Count - 1;
        }

        return source[number];
    }

    /// <summary>
    /// ���g�p�̃\�[�X���擾����
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private AudioSource GetUnusedSESource(List<AudioSource> source)
    {
        int number = -1;
        for (int i = 0, max = source.Count; i < max; i++)
        {
            if (source[i].time > 0) continue;

            number = i;
            break;
        }
        if (number == -1)
        {
            source.Add(Instantiate(_seSourceOrigin, transform).GetComponent<AudioSource>());
            number = source.Count - 1;
        }

        return source[number];
    }
}
