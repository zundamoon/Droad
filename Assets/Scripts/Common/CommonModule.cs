using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommonModule
{
    // ステージの位置情報を管理
    public struct StagePosition
    {
        public int route;
        public int road;
        public int square;

        public StagePosition(int route_, int road_, int square_)
        {
            route = route_;
            road = road_;
            square = square_;
        }
    }

    /// <summary>
	/// リストの初期化
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
	/// 配列が空か否か
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static bool IsEmpty<T>(T[] array)
    {
        return array == null || array.Length == 0;
    }

    /// <summary>
    /// 配列に対して有効なインデクスか否か
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
    /// リストが空か否か
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkList"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> checkList)
    {
        return checkList == null || checkList.Count == 0;
    }

    /// <summary>
	/// リストに対して有効なインデクスか否か
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
    /// リストを重複なしでマージ
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
    /// キューが空か否か
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkQueue"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(Queue<T> checkQueue)
    {
        return checkQueue == null || checkQueue.Count == 0;
    }

    /// <summary>
    /// リスト1からリスト2に要素を移動させる
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
    /// リスト1からリスト2に指定した要素を移動させる
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
    /// 指定した秒数後に関数を実行する
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
    /// 指定した秒数後に関数を実行する(キャンセル可)
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
    /// 指定した秒数後に関数を実行する
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
    /// 指定した秒数後に関数を実行する
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
    /// 指定した秒数後に関数を実行する
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
    /// 指定したフレームに関数を実行する
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
    /// 指定したフレームに関数を実行する(キャンセル可能)
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
    /// 指定したフレームに関数を実行する
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
    /// 指定したフレームに関数を実行する
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
    /// 指定したフレームに関数を実行する
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

