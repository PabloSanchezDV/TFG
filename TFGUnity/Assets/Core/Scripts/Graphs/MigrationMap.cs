using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Grid;
using static Radar;

public class MigrationMap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pinPrefab;

    [Header("Data")]
    [SerializeField] private List<Route> requiredRoutes;

    [Header("Grid Settings")]
    [SerializeField] private int horizontalTiles;
    [SerializeField] private int verticalTiles;
    [SerializeField] private Grid grid;

    private void Start()
    {
        foreach(Route route in requiredRoutes)
        {
            int order = 0;
            foreach(RouteTile routeTile in route.Tiles)
            {
                grid.AddRequiredSocket(new RequiredSocket(route.PinType, new PinSocketPosition(routeTile.Tile.Row, routeTile.Tile.Column), order));
                order++;
            }
        }

        grid.CreateGrid(horizontalTiles, verticalTiles, NotifyMigrationMapCompleted);
    }

    public void UpdateStartingPins(List<Route> registeredRoutes)
    {
        if(registeredRoutes != null)
            StartCoroutine(UpdateStartingPinsCoroutine(registeredRoutes));
    }

    private IEnumerator UpdateStartingPinsCoroutine(List<Route> registeredRoutes)
    {
        Time.timeScale = 0.1f;
        foreach (Route route in registeredRoutes)
        {
            foreach (RouteTile routeTile in route.Tiles)
            {
                grid.ForceAddPin(pinPrefab, route.PinType, routeTile.Tile.Row, routeTile.Tile.Column);
                yield return null;
            }
            yield return null;
        }
        Time.timeScale = 1f;
    }

    private void NotifyMigrationMapCompleted()
    {
        GameManager.Instance.OnMigrationMapCompleted?.Invoke();
    }

    public void AddRequiredRoute(Route requiredRoute)
    {
        int order = 0;

        foreach(RouteTile tile in requiredRoute.Tiles)
        {
            RequiredSocket requiredSocket = new RequiredSocket(requiredRoute.PinType, new PinSocketPosition(tile.Tile.Row, tile.Tile.Column), order);
            grid.AddRequiredSocket(requiredSocket);
            order++;
        }
    }

    public bool IsMigrationCompleted()
    {
        return grid.IsComplete();
    }
}
