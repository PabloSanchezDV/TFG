using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    [Header("Bird Icon Settings")]
    [SerializeField] private RectTransform birdIcon;
    [SerializeField] private float birdIconRotationOffset;

    [Header("Tiling Values")]
    [SerializeField] private Tile currentTile;
    [SerializeField] private float tileSize;

    [Header("Migration Values")]
    [SerializeField] private Tile startingTile;
    [SerializeField] private List<RouteTile> tilesRoute;
    [SerializeField] private float speed;

    private Material material;
    private Tile targetTile;
    private Vector2 movement;
    private bool migrationEnded;
    private bool goToTileCenter;
    private bool performingStop;

    private void Start()
    {
        Image image = GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        material.mainTextureOffset = GetTilePosition(startingTile);

        SetDestination(tilesRoute[0].Tile);
    }

    public void UpdateMovement()
    {
        if (!migrationEnded && !performingStop)
        {
            material.mainTextureOffset += movement * Time.deltaTime;

            if (!goToTileCenter)
                CheckForTileChange();
            else
                CheckForTileCenter();
        }
    }

    private Vector2 GetTilePosition(Tile tile)
    {
        return new Vector2(tile.Column * tileSize, tile.Row * tileSize);
    }

    private void SetDestination(Tile tile)
    {
        movement = (GetTilePosition(tile) - material.mainTextureOffset).normalized * speed;
        birdIcon.localRotation = Quaternion.Euler(0, 0, GetAngle(movement) + birdIconRotationOffset);
        targetTile = tile;
    }

    private bool IsTargetTile(Tile tile)
    {
        if(tile.Equals(targetTile)) 
            return true;
        return false;
    }

    private void CheckForTileChange()
    {
        int detectedColumn = Mathf.FloorToInt((material.mainTextureOffset.x + tileSize * 0.5f) / tileSize);
        int detectedRow = Mathf.FloorToInt((material.mainTextureOffset.y + tileSize * 0.5f) / tileSize);

        Tile detectedTile = new Tile(detectedRow, detectedColumn);

        bool triggerAlarm = false;

        if (!detectedTile.Equals(currentTile))
        {
            currentTile = detectedTile;

            if (IsTargetTile(currentTile))
                goToTileCenter = true;

            triggerAlarm = true;
        }

        if (triggerAlarm)
            Debug.Log("Change detected");
    }

    private void CheckForTileCenter()
    {
        int detectedColumn = Mathf.FloorToInt((material.mainTextureOffset.x) / tileSize);
        int detectedRow = Mathf.FloorToInt((material.mainTextureOffset.y) / tileSize);

        Tile detectedTile = new Tile(detectedRow, detectedColumn);

        if (detectedTile.Equals(currentTile))
        {
            RouteTile currentRouteTile = tilesRoute.Find(routeTile => routeTile.Tile.Equals(currentTile));

            if (tilesRoute.IndexOf(currentRouteTile) < tilesRoute.Count - 1)
                StartCoroutine(PerformArrivalStop(currentRouteTile, tilesRoute[tilesRoute.IndexOf(currentRouteTile) + 1].Tile));                
            else
            {
                migrationEnded = true;
                Debug.Log("Migration ended");
                return;
            }

            goToTileCenter = false;
            Debug.Log("Center detected");
        }
    }

    private IEnumerator PerformArrivalStop(RouteTile routeTile, Tile nextTile)
    {
        performingStop = true;
        yield return new WaitForSeconds(routeTile.StopTimeOnArrival);
        SetDestination(nextTile);
        performingStop = false;
    }

    private float GetAngle(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;

        return angle;
    }

    [Serializable]
    private class Tile
    {
        [SerializeField] private int row;
        [SerializeField] private int column;

        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public Tile(int row, int column) 
        {
            this.row = row;
            this.column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Tile other)
                return false;
            return row == other.row && column == other.column;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(row, column);
        }
    }

    [Serializable]
    private class RouteTile
    {
        [SerializeField] private Tile tile;
        [SerializeField] private float stopTimeOnArrival;

        public Tile Tile { get { return tile; } }
        public float StopTimeOnArrival { get { return stopTimeOnArrival; } }
    }
}
