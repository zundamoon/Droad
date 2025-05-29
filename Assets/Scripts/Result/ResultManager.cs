using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private List<Color> _playerColors = null;

    [SerializeField]
    private GameObject _playerObject = null;

    [SerializeField]
    private List<Transform> _playerAnchors = null;

    private void Start()
    {
        SetPlayer();
    }

    private void SetPlayer()
    {
        // キャラクターの順位で色付け
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            GameObject playerObj = Instantiate(_playerObject, _playerAnchors[i].position, Quaternion.identity);
            int rank = SendData.rankList[i];
            playerObj.GetComponent<MeshRenderer>().material.color = _playerColors[rank];
        }
    }
}
