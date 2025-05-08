using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class SquareList
    {
        public List<GameObject> squareArray = new List<GameObject>();
    }

    [System.Serializable]
    public class RoadList
    {
        public List<SquareList> routeList = new List<SquareList>();
    }

    [System.Serializable]
    public class RouteList
    {
        public List<RoadList> routeList = new List<RoadList>();
    }

    public RouteList routeList_ = new RouteList();
}
