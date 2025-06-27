using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private List<TextMeshPro> _starText = null;

    [SerializeField]
    private List<TextMeshPro> _coinText = null;

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
            int rank = SendData.rankList[i][0];
            int coins = SendData.rankList[i][1];
            int stars = SendData.rankList[i][2];
            _starText[i].text = stars.ToString();
            _coinText[i].text = coins.ToString();
            playerObj.GetComponent<MeshRenderer>().material.color = _playerColors[rank];
        }
    }
}
