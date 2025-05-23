using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using static CommonModule;

public class MenuStatus : BaseMenu
{
    /// <summary>
    /// リスト項目のオリジナル
    /// </summary>
    [SerializeField]
    private MenuStatusChara _charaStatusOrigin = null;
    [SerializeField]
    private MenuStatusEvent _eventStatusOrigin = null;

    /// <summary>
    /// 並べる背景
    /// </summary>
    [SerializeField]
    private RectTransform _BGRect = null;
    /// <summary>
    /// 項目を並べるルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;
    /// <summary>
    /// 未使用状態の項目のルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    [SerializeField]
    private RectTransform _startPos = null;

    // 順番リスト
    private List<MenuStatusItem> _statusOrderList = null;

    private List<MenuStatusChara> _useCharaList = null;
    private List<MenuStatusChara> _unuseCharaList = null;
    private List<MenuStatusEvent> _useEventList = null;
    private List<MenuStatusEvent> _unuseEventList = null;

    private const int _STATUS_MAX = 5;
    private const int _EVENT_MAX = 3;

    private List<Character> _characters = null;

    public void SetCharacter(List<Character> setCharacters)
    {
        _characters = setCharacters;
    }

    public async UniTask Alignment()
    {
        if (!IsEnableIndex(_statusOrderList, 0)) return;

        List<UniTask> tasks = new List<UniTask>();

        Vector3 pos = Vector3.zero;
        // スペース
        pos.y -= 20;

        for (int i = 0; i < _statusOrderList.Count; i++)
        {
            var status = _statusOrderList[i];

            UniTask task = status.Move(pos);
            tasks.Add(task);

            pos.y -= status.GetHeight();
            // スペース
            pos.y -= 20;
        }

        await UniTask.WhenAll(tasks);
    }

    public async UniTask ScrollStatus()
    {
        if (!IsEnableIndex(_statusOrderList, 0)) return;
        await UIManager.instance.CloseCardText();

        _statusOrderList[0].ReSize(0.8f);
        _statusOrderList.RemoveAt(0);
        RemoveCharaStatus(0);

        if (IsEnableIndex(_statusOrderList, 0) &&
            _statusOrderList[0].isChara == false)
        {
            _statusOrderList[0].ReSize(0.8f);
            RemoveEventStatus(0);
            _statusOrderList.RemoveAt(0);
        }
        await Alignment();
    }

    public async UniTask AddStatus(Character character)
    {
        var status = AddCharaStatusItem();
        status.SetChara(character);
        _statusOrderList.Add(status);

        await Alignment();
    }

    public async UniTask AddStatus(string str)
    {
        var status = AddEventStatusItem();
        status.SetEvent(str);
        _statusOrderList.Add(status);

        await Alignment();
    }

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useCharaList = new List<MenuStatusChara>();
        _unuseCharaList = new List<MenuStatusChara>();
        for (int i = 0; i < _STATUS_MAX; i++)
        {
            var status = Instantiate(_charaStatusOrigin, _unuseRoot);
            _unuseCharaList.Add(status);
        }
        _useEventList = new List<MenuStatusEvent>();
        _unuseEventList = new List<MenuStatusEvent>();
        for (int i = 0; i < _EVENT_MAX; i++)
        {
            var status = Instantiate(_eventStatusOrigin, _unuseRoot);
            _unuseEventList.Add(status);
        }
        _statusOrderList = new List<MenuStatusItem>();
        _characters = new List<Character>();
    }

    /// <summary>
    /// リスト項目の生成
    /// </summary>
    /// <returns></returns>
    private MenuStatusChara AddCharaStatusItem()
    {
        MenuStatusChara addItem;
        if (IsEmpty(_unuseCharaList))
        {
            // 未使用リストが空なので生成
            addItem = Instantiate(_charaStatusOrigin, _contentRoot);
        }
        else
        {
            // 未使用リストから取得
            addItem = _unuseCharaList[0];
            _unuseCharaList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        addItem.Initialize();
        addItem.transform.position = _startPos.position;
        _useCharaList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// リスト項目の生成
    /// </summary>
    /// <returns></returns>
    private MenuStatusEvent AddEventStatusItem()
    {
        MenuStatusEvent addItem;
        if (IsEmpty(_unuseEventList))
        {
            // 未使用リストが空なので生成
            addItem = Instantiate(_eventStatusOrigin, _contentRoot);
        }
        else
        {
            // 未使用リストから取得
            addItem = _unuseEventList[0];
            _unuseEventList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        addItem.Initialize();
        addItem.transform.position = _startPos.position;
        _useEventList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// インデクス指定のリスト項目削除
    /// </summary>
    /// <param name="itemIndex"></param>
    private void RemoveCharaStatus(int itemIndex)
    {
        if (!IsEnableIndex(_useCharaList, itemIndex)) return;
        // 使用リストから取り除く
        MenuStatusChara removeItem = _useCharaList[itemIndex];
        _useCharaList.RemoveAt(itemIndex);
        // 未使用リストへ追加
        _unuseCharaList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }
    /// <summary>
    /// インデクス指定のリスト項目削除
    /// </summary>
    /// <param name="itemIndex"></param>
    private void RemoveEventStatus(int itemIndex)
    {
        if (!IsEnableIndex(_useEventList, itemIndex)) return;
        // 使用リストから取り除く
        MenuStatusEvent removeItem = _useEventList[itemIndex];
        _useEventList.RemoveAt(itemIndex);
        // 未使用リストへ追加
        _unuseEventList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// 全てのリスト項目削除
    /// </summary>
    public async UniTask RemoveAllStatus()
    {
        while (!IsEmpty(_useCharaList)) RemoveCharaStatus(0);
        while (!IsEmpty(_useEventList)) RemoveEventStatus(0);
        while(!IsEmpty(_statusOrderList)) _statusOrderList.Clear();

        await UniTask.DelayFrame(1);
    }

    /// <summary>
    /// リスト項目のサイズ変更
    /// </summary>
    /// <returns></returns>
    public async UniTask ReSizeTop()
    {
       await _statusOrderList[0].ReSize(1);
    }

    /// <summary>
    /// 全てのリスト項目をスクロール
    /// </summary>
    /// <returns></returns>
    public async UniTask ScrollAllStatus()
    {
        for (int i = 0; i < _STATUS_MAX + _EVENT_MAX; i++)
        {
            await ScrollStatus();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public async UniTask SetCharaStatus(int playerID)
    {
        if (!IsEnableIndex(_characters, playerID)) return;
        for (int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i].playerID == playerID)
            {
                await AddStatus(_characters[i]);
                break;
            }
        }
    }

    /// <summary>
    /// 情報を更新する
    /// </summary>
    /// <param name="chara"></param>
    /// <returns></returns>
    public async UniTask UpdateStatus(Character chara)
    {
        for (int i = 0; i < _useCharaList.Count; i++)
        {
            if (chara.playerID == _useCharaList[i]._charaID)
            {
                _useCharaList[i].SetChara(chara);
                break;
            }
        }
    }
}
