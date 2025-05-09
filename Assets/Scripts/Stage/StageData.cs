using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    [System.Serializable]
    public class SquareList
    {
        public List<GameObject> squareList = new List<GameObject>();
    }

    [System.Serializable]
    public class RoadList
    {
        public List<SquareList> roadList = new List<SquareList>();
    }

    [System.Serializable]
    public class RouteList
    {
        public List<RoadList> routeList = new List<RoadList>();
    }

    public RouteList stageRoute = new RouteList();
}
