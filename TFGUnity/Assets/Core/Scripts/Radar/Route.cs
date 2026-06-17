using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Radar;

[CreateAssetMenu(fileName = "Route", menuName = "Scriptable Objects/Route")]
public class Route : ScriptableObject
{
    [SerializeField] private RouteData routeData;

    public List<RouteTile> Tiles { get { return routeData.RouteTiles.ToList(); } }
    public PinType PinType { get { return routeData.PinType; } }

    [Serializable]
    private class RouteData
    {
        [SerializeField] private PinType pinType;
        [SerializeField] private List<RouteTile> tiles;

        public PinType PinType { get { return pinType; } }
        public List<RouteTile> RouteTiles { get { return tiles; } }
    }
}
