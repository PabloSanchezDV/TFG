using UnityEngine;

public class MigrationMap : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int horizontalTiles;
    [SerializeField] private int verticalTiles;
    [SerializeField] private Grid grid;

    private void Start()
    {
        grid.CreateGrid(horizontalTiles, verticalTiles, ConnectPoints);
    }

    private void ConnectPoints()
    {
        Debug.Log("Conecting points...");
    }
}
