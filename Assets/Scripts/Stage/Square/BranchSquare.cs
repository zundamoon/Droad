using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BranchSquare : BaseSquareData
{
    [SerializeField] public GameObject branchArrow;
    private List<GameObject> _generatedObjectList;
    public BranchSquare()
    {
        squareColor = Color.white;
        eventID = 9;
        _generatedObjectList = new List<GameObject>();
    }

    public async UniTask SelectBranch(Character targetCharacter)
    {
        // 分岐先のindex
        int index = -1;

        for (int i = 0; i < nextPositionList.Count; i++)
        {
            Vector3 objectPos = StageManager.instance.GetPosition(nextPositionList[i]);
            objectPos.y += 3;
            GameObject arrowObject = MonoBehaviour.Instantiate(branchArrow, objectPos, Quaternion.identity);
            ArrowData arrowData = arrowObject.GetComponent<ArrowData>();
            arrowData.nextPosition = nextPositionList[i];
            arrowData.number = i;
            _generatedObjectList.Add(arrowObject);
        }

        // 入力待ち
        while(true)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // レイを飛ばしてヒットしたかチェック
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.tag == "Arrow")
                    {
                        ArrowData arrowData = hit.transform.GetComponent<ArrowData>();
                        index = arrowData.number;
                        break;
                    }
                }
            }

            await UniTask.Yield();
        }

        targetCharacter.nextPosition = nextPositionList[index];

        for (int i = 0; i < _generatedObjectList.Count; i++)
        {
            MonoBehaviour.Destroy(_generatedObjectList[i]);
        }

    }
}


