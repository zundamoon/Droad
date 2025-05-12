using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommonModule
{
    /// <summary>
    /// �X�e�[�W��̈ʒu����\���\����
    /// </summary>
    public struct StagePosition
    {
        public int m_route;
        public int m_road;
        public int m_square;

        // �R���X�g���N�^
        public StagePosition(int route, int road, int square)
        {
            m_route = route;
            m_road = road;
            m_square = square;
        }

        /// <summary>
        /// �I�u�W�F�N�g�������������r
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is StagePosition)) return false;

            StagePosition other = (StagePosition)obj;

            return m_route == other.m_route &&
                   m_road == other.m_road &&
                   m_square == other.m_square;
        }

        /// <summary>
        /// �n�b�V���R�[�h���擾
        /// </summary>
        public override int GetHashCode()
        {
            return (m_route, m_road, m_square).GetHashCode();
        }

        /// <summary>
        /// �������Z�q
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(StagePosition a, StagePosition b) => a.Equals(b);

        /// <summary>
        /// �񓙉����Z�q
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(StagePosition a, StagePosition b) => !a.Equals(b);
    }

    /// <summary>
	/// ���X�g�̏�����
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="capacity"></param>
	public static void InitializeList<T>(ref List<T> list, int capacity = -1)
    {
        if (list == null)
        {
            if (capacity < 0)
            {
                list = new List<T>();
            }
            else
            {
                list = new List<T>(capacity);
            }
        }
        else
        {
            if (list.Capacity < capacity) list.Capacity = capacity;

            list.Clear();
        }
    }

    /// <summary>
	/// �z�񂪋󂩔ۂ�
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static bool IsEmpty<T>(T[] array)
    {
        return array == null || array.Length == 0;
    }

    /// <summary>
    /// �z��ɑ΂��ėL���ȃC���f�N�X���ۂ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(T[] array, int index)
    {
        if (IsEmpty(array)) return false;

        return array.Length > index && index >= 0;
    }

    /// <summary>
    /// ���X�g���󂩔ۂ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkList"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> checkList)
    {
        return checkList == null || checkList.Count == 0;
    }

    /// <summary>
	/// ���X�g�ɑ΂��ėL���ȃC���f�N�X���ۂ�
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static bool IsEnableIndex<T>(List<T> list, int index)
    {
        if (IsEmpty(list)) return false;

        return list.Count > index && index >= 0;
    }

    /// <summary>
    /// ���X�g���d���Ȃ��Ń}�[�W
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    public static void MeargeList<T>(ref List<T> main, List<T> sub)
    {
        if (IsEmpty(sub)) return;

        if (main == null) main = new List<T>();

        for (int i = 0, max = sub.Count; i < max; i++)
        {
            if (main.Exists(mainElem => mainElem.Equals(sub[i]))) continue;

            main.Add(sub[i]);
        }
    }

    /// <summary>
    /// �L���[���󂩔ۂ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkQueue"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(Queue<T> checkQueue)
    {
        return checkQueue == null || checkQueue.Count == 0;
    }

    /// <summary>
    /// ���X�g1���烊�X�g2�ɗv�f���ړ�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceList"></param>
    /// <param name="targetList"></param>
    public static void ReinsertListMember<T>(ref List<T> sourceList, ref List<T> targetList)
    {
        if (IsEmpty(sourceList)) return;

        int targetNum = sourceList.Count - 1;
        T member = sourceList[targetNum];
        sourceList.RemoveAt(targetNum);
        targetList.Add(member);
    }

    /// <summary>
    /// ���X�g1���烊�X�g2�Ɏw�肵���v�f���ړ�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceList"></param>
    /// <param name="targetList"></param>
    /// <param name="target"></param>
    public static void ReinsertListMember<T>(ref List<T> sourceList, ref List<T> targetList, T target)
    {
        if (IsEmpty(sourceList)) return;

        int targetNum = sourceList.IndexOf(target);
        if (targetNum < 0) return;

        T member = sourceList[targetNum];
        sourceList.RemoveAt(targetNum);
        targetList.Add(member);
    }

    #region WaitAction(sec)

    /// <summary>
    /// �w�肵���b����Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(float sec, System.Action action)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        action();
    }

    /// <summary>
    /// �w�肵���b����Ɋ֐������s����(�L�����Z����)
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(float sec, System.Action action, CancellationToken token)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1, cancellationToken: token);
        }
        action();
    }

    /// <summary>
    /// �w�肵���b����Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction<T>(float sec, System.Action<T> action, T pram)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            pram != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        action(pram);
    }

    /// <summary>
    /// �w�肵���b����Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(float sec, System.Func<T> action)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        return action();
    }

    /// <summary>
    /// �w�肵���b����Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(float sec, System.Func<T, T> action, T pram)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            pram != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        return action(pram);
    }

    #endregion

    #region WaitAction(frame)

    /// <summary>
    /// �w�肵���t���[���Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(int frame, System.Action action)
    {
        float elapsedFrame = 0;
        while (action != null &&
            elapsedFrame < frame)
        {
            elapsedFrame += Time.timeScale;
            await UniTask.DelayFrame(1);
        }
        action();
    }

    /// <summary>
    /// �w�肵���t���[���Ɋ֐������s����(�L�����Z���\)
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(int frame, System.Action action, CancellationToken token)
    {
        float elapsedFrame = 0;
        while (action != null &&
            elapsedFrame < frame)
        {
            elapsedFrame += Time.timeScale;
            await UniTask.DelayFrame(1, cancellationToken: token);
        }
        action();
    }

    /// <summary>
    /// �w�肵���t���[���Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction<T>(int frame, System.Action<T> action, T pram)
    {
        float elapsedFrame = 0;
        while (action != null &&
            pram != null &&
            elapsedFrame < frame)
        {
            elapsedFrame += Time.timeScale;
            await UniTask.DelayFrame(1);
        }
        action(pram);
    }

    /// <summary>
    /// �w�肵���t���[���Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(int frame, System.Func<T> action)
    {
        float elapsedFrame = 0;
        while (action != null &&
            elapsedFrame < frame)
        {
            elapsedFrame += Time.timeScale;
            await UniTask.DelayFrame(1);
        }
        return action();
    }

    /// <summary>
    /// �w�肵���t���[���Ɋ֐������s����
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(int frame, System.Func<T, T> action, T pram)
    {
        float elapsedFrame = 0;
        while (action != null &&
            pram != null &&
            elapsedFrame < frame)
        {
            elapsedFrame += Time.timeScale;
            await UniTask.DelayFrame(1);
        }
        return action(pram);
    }

    #endregion
}

