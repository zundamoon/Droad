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
        // UI�̃f�[�^���擾
        for (int i = 0; i < countUIObjectList.Count; i++)
        {
            BaseCountUI countUI = countUIObjectList[i].GetComponent<BaseCountUI>();
            countUI.Init();
            countUIList.Add(countUI);
        }
    }

    /// <summary>
    /// �ݒ芮���{�^���������ꂽ���̏���
    /// </summary>
    public void SettingDecide()
    {
        // UI�ɐݒ肳�ꂽ�l���擾���ĕێ�����
        for (int i = 0; i < countUIList.Count; i++)
        {
            BaseCountUI countUI = countUIList[i];

            if(countUI is PlayerCountUI) _gameData.settingPlayerCount = countUI.GetCount();
            else if(countUI is StageIDCountUI) _gameData.settingStageID = countUI.GetCount();
            else if(countUI is TurnCountUI) _gameData.settingTurnCount = countUI.GetCount();
        }

        // �V�[���J��
        FadeSceneChange.ChangeSceneEvent(_NEXT_SCENE_NAME);
    }
}
