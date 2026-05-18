using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject socketPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private float zValue;
    [SerializeField] private bool filterPinType;
    [SerializeField] private List<RequiredSocket> requiredSockets;

    private Dictionary<PinSocketPosition, PinSocket> pinSocketsDictionary;
    private Dictionary<PinSocketPosition, bool> occupiedPinSocketPositionsDictionary;

    private Action onCompleteGrid;

    public void CreateGrid(int xEntries, int yEntries, Action onCompleteGrid)
    {
        occupiedPinSocketPositionsDictionary = new Dictionary<PinSocketPosition, bool>();
        pinSocketsDictionary = new Dictionary<PinSocketPosition, PinSocket>();
        this.onCompleteGrid = onCompleteGrid;

        float xSize = container.sizeDelta.x / xEntries;
        float ySize = container.sizeDelta.y / yEntries;

        for (int i = 0; i < xEntries; i++)
        {
            for (int j = 0; j < yEntries; j++)
            {
                GameObject socketGO = CreatePinSocket(new Vector3(i * xSize + xSize * 0.5f, j * ySize + ySize * 0.5f, zValue), j, i);
                PinSocketPosition pinSocketPosition = new PinSocketPosition(i, j);
                pinSocketsDictionary.Add(pinSocketPosition, socketGO.GetComponent<PinSocket>());
            }
        }
    }

    private GameObject CreatePinSocket(Vector3 position, int row, int column)
    {
        GameObject pinSocketGO = Instantiate(socketPrefab, container, false);
        pinSocketGO.name = $"PinSocket_{row}_{column}";
        pinSocketGO.transform.localPosition = position;
        pinSocketGO.transform.localRotation = Quaternion.identity;
        PinSocket pinSocket = pinSocketGO.GetComponent<PinSocket>();
        pinSocket.onOccupyAction = AddOccupiedPoint;
        pinSocket.onDeoccupyAction = RemoveOccupiedPoint;
        pinSocket.Row = row;
        pinSocket.Column = column;
        return pinSocketGO;
    }

    private void AddOccupiedPoint(PinType pinType, int row, int column)
    {
        PinSocketPosition pinSocketPosition = new PinSocketPosition(row, column);
        bool isRequiredPin;
        
        if(filterPinType)
            isRequiredPin = requiredSockets.Find(e => e.PinSocketPosition.Equals(pinSocketPosition) && e.PinType == pinType) != null;
        else
            isRequiredPin = requiredSockets.Find(e => e.PinSocketPosition == pinSocketPosition) != null;
        
        occupiedPinSocketPositionsDictionary.Add(pinSocketPosition, isRequiredPin);
        CheckPins();
    }

    private void RemoveOccupiedPoint(int row, int column)
    {
        PinSocketPosition pinSocketPosition = new PinSocketPosition(row, column);
        if (occupiedPinSocketPositionsDictionary.TryGetValue(pinSocketPosition, out bool value))
        {
            occupiedPinSocketPositionsDictionary.Remove(pinSocketPosition);
        }
        else
            throw new Exception("Trying to remove a non listed PinSocket");
    }

    private void CheckPins()
    {
        if (occupiedPinSocketPositionsDictionary.Count != requiredSockets.Count)
            return;

        bool chartCompleted = true;
        foreach (bool dictValue in occupiedPinSocketPositionsDictionary.Values)
        {
            if (dictValue == false)
            {
                chartCompleted = false;
                break;
            }
        }

        if (chartCompleted)
            onCompleteGrid();
    }

    [Serializable]
    private class PinSocketPosition
    {
        public int row;
        public int column;

        public PinSocketPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is not PinSocketPosition other)
                return false;
            return row == other.row && column == other.column;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(row, column);
        }
    }

    [Serializable]
    private class RequiredSocket
    {
        [SerializeField] private PinType pinType;
        [SerializeField] private PinSocketPosition pinSocketPosition;

        public PinType PinType { get { return pinType; } }
        public PinSocketPosition PinSocketPosition { get { return pinSocketPosition; } }
    }
}
