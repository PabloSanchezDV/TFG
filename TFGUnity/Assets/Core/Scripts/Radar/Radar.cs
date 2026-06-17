using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private MeshRenderer lightMeshRenderer;
    [SerializeField] private float enabledLightTime;

    [Header("Bird Icon Settings")]
    [SerializeField] private RectTransform birdIcon;
    [SerializeField] private TextMeshProUGUI memberDisplayText;
    [SerializeField] private Color birdColor;
    [SerializeField] [ColorUsage(true, true)] private Color lightColor;
    [SerializeField] private float birdIconRotationOffset;

    [Header("Tiling Values")]
    [SerializeField] private Tile currentTile;
    [SerializeField] private Vector2 tileSize;

    [Header("Migration Values")]
    [SerializeField] private Route tilesRoute;
    [SerializeField] private float speed;

    private Image image;
    private Image birdIconImage;
    private Material lightMaterial;
    private Material mapMaterial;
    private Tile targetTile;
    private Vector2 movement;
    private bool migrationEnded;
    private bool goToTileCenter;
    private bool performingStop;
    private bool canMove;

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    public void Initialize(bool canMove, bool disableImage = true)
    {
        this.canMove = canMove;

        image = GetComponentInChildren<Image>();
        birdIconImage = birdIcon.GetComponent<Image>();

        mapMaterial = new Material(image.material);
        image.material = mapMaterial;

        lightMaterial = new Material(lightMeshRenderer.material);
        lightMeshRenderer.material = lightMaterial;
        lightMaterial.SetColor("_BaseColor", birdColor);
        lightMaterial.SetColor("_EmissionColor", lightColor);
        lightMaterial.DisableKeyword("_EMISSION");

        mapMaterial.mainTextureOffset = GetTilePosition(tilesRoute.Tiles[0].Tile);
        image.gameObject.SetActive(disableImage);

        SetDestination(tilesRoute.Tiles[0].Tile);
    }

    public void EnableImage()
    {
        birdIconImage.color = birdColor;
        memberDisplayText.color = birdColor;
        image.gameObject.SetActive(true);
    }

    public void DisableImage()
    {
        image.gameObject.SetActive(false);
    }

    public void UpdateMovement()
    {
        if (canMove && !migrationEnded && !performingStop)
        {
            mapMaterial.mainTextureOffset += movement * Time.deltaTime;

            if (!goToTileCenter)
                CheckForTileChange();
            else
                CheckForTileCenter();
        }
    }

    private Vector2 GetTilePosition(Tile tile)
    {
        return new Vector2(tile.Column * tileSize.x, tile.Row * tileSize.y);
    }

    private void SetDestination(Tile tile)
    {
        movement = (GetTilePosition(tile) - mapMaterial.mainTextureOffset).normalized * speed;
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
        int detectedColumn = Mathf.FloorToInt((mapMaterial.mainTextureOffset.x + tileSize.x * 0.5f) / tileSize.x);
        int detectedRow = Mathf.FloorToInt((mapMaterial.mainTextureOffset.y + tileSize.y * 0.5f) / tileSize.y);

        Tile detectedTile = new Tile(detectedRow, detectedColumn);

        if (!detectedTile.Equals(currentTile))
        {
            currentTile = detectedTile;

            if (IsTargetTile(currentTile))
                goToTileCenter = true;
        }            
    }

    public void TriggerAlarm()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayMigrationAlarm();

        StartCoroutine(TriggerLightForTime());
    }

    IEnumerator TriggerLightForTime()
    {
        lightMaterial.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(enabledLightTime);
        lightMaterial.DisableKeyword("_EMISSION");
    }

    private void CheckForTileCenter()
    {
        int detectedColumn = Mathf.FloorToInt((mapMaterial.mainTextureOffset.x) / tileSize.x);
        int detectedRow = Mathf.FloorToInt((mapMaterial.mainTextureOffset.y) / tileSize.y);

        Tile detectedTile = new Tile(detectedRow, detectedColumn);

        if (detectedTile.Equals(currentTile))
        {
            RouteTile currentRouteTile = tilesRoute.Tiles.Find(routeTile => routeTile.Tile.Equals(currentTile));

            if (tilesRoute.Tiles.IndexOf(currentRouteTile) < tilesRoute.Tiles.Count - 1)
                StartCoroutine(PerformArrivalStop(currentRouteTile, tilesRoute.Tiles[tilesRoute.Tiles.IndexOf(currentRouteTile) + 1].Tile));                
            else
            {
                migrationEnded = true;
                TriggerAlarm();
                return;
            }

            goToTileCenter = false;
            TriggerAlarm();
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
    public class Tile
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
    public class RouteTile
    {
        [SerializeField] private Tile tile;
        [SerializeField] private float stopTimeOnArrival;

        public Tile Tile { get { return tile; } }
        public float StopTimeOnArrival { get { return stopTimeOnArrival; } }
    }
}
