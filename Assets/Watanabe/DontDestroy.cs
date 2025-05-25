/*
 * @file DontDestroy.cs
 * @brief �V�[�����܂����Ŏg�������I�u�W�F�N�g��ݒ肷��
 * @author sein
 * @date 2025/1/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy[] instance = new DontDestroy[1];
    public int ObjectNum;

    void Awake()
    {
        CheckInstance();
    }

    // dontdestroy�𕡐��g����悤�ɂ��ăI�u�W�F�N�g�ԍ������Ԃ��Ă������
    void CheckInstance()
    {
        if (instance[ObjectNum] == null)
        {
            instance[ObjectNum] = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

