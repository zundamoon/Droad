using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameEnum;

public class GameSettingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> countUIObjectList;
    private List<BaseCountUI> countUIList = new List<BaseCountUI>();
    [SerializeField] private GameData _gameData = null;

    private const string _NEXT_SCENE_NAME = "MainGame";

    private void Start()
    {
        // UIのデータを取得
        for (int i = 0; i < countUIObjectList.Count; i++)
        {
            BaseCountUI countUI = countUIObjectList[i].GetComponent<BaseCountUI>();
            countUI.Init();
            countUIList.Add(countUI);
        }
    }

    /// <summary>
    /// 設定完了ボタンが押された時の処理
    /// </summary>
    public void SettingDecide()
    {
        // UIに設定された値を取得して保持する
        for (int i = 0; i < countUIList.Count; i++)
        {
            BaseCountUI countUI = countUIList[i];

            if(countUI is PlayerCountUI) _gameData.settingPlayerCount = countUI.GetCount();
            else if(countUI is StageIDCountUI) _gameData.settingStageID = countUI.GetCount();
            else if(countUI is TurnCountUI) _gameData.settingTurnCount = countUI.GetCount();
        }

        // シーン遷移
        FadeSceneChange.ChangeSceneEvent(_NEXT_SCENE_NAME);
    }
}
