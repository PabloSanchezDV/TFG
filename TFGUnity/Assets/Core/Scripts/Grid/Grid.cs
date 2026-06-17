using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject socketPrefab;
    [SerializeField] private GameObject threadPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private float zValue;
    [SerializeField] private bool filterPinType;
    [SerializeField] private List<RequiredSocket> requiredSockets;
    [SerializeField] private bool connectPoints;
    [SerializeField] private Vector3 threadRotationOffset;
    [SerializeField] private Vector3 pinSocketRotation;
    [SerializeField] private bool playAudioOnWrongPinPosition;

    [Header("Pin Colors")]
    [SerializeField] private Color red;
    [SerializeField] private Color white;
    [SerializeField] private Color blue;
    [SerializeField] private Color yellow;
    [SerializeField] private Color purple;
    [SerializeField] private Color pink;
    [SerializeField] private Color orange;
    [SerializeField] private Color green;

    float xSize;
    float ySize;

    private Dictionary<PinSocketPosition, PinSocket> pinSocketsDictionary;
    private Dictionary<PinSocketPosition, bool> occupiedPinSocketPositionsDictionary;

    private Action onCompleteGrid;

    public void CreateGrid(int xEntries, int yEntries, Action onCompleteGrid)
    {
        occupiedPinSocketPositionsDictionary = new Dictionary<PinSocketPosition, bool>();
        pinSocketsDictionary = new Dictionary<PinSocketPosition, PinSocket>();
        this.onCompleteGrid = onCompleteGrid;

        xSize = container.rect.width / xEntries;
        ySize = container.rect.height / yEntries;

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
        pinSocketGO.transform.localRotation = Quaternion.Euler(pinSocketRotation);
        PinSocket pinSocket = pinSocketGO.GetComponent<PinSocket>();
        pinSocket.onOccupyAction = AddOccupiedPoint;
        pinSocket.onDeoccupyAction = RemoveOccupiedPoint;
        pinSocket.Row = row;
        pinSocket.Column = column;
        return pinSocketGO;
    }

    public void AddRequiredSocket(RequiredSocket requiredSocket)
    {
        requiredSockets.Add(requiredSocket);
    }

    private void AddOccupiedPoint(PinType pinType, int row, int column, bool checkPins = true)
    {
        PinSocketPosition pinSocketPosition = new PinSocketPosition(row, column);
        bool isRequiredPin;
        
        if(filterPinType)
            isRequiredPin = requiredSockets.Find(e => e.PinSocketPosition.Equals(pinSocketPosition) && e.PinType == pinType) != null;
        else
            isRequiredPin = requiredSockets.Find(e => e.PinSocketPosition.Equals(pinSocketPosition)) != null;

        if (playAudioOnWrongPinPosition && !isRequiredPin && AudioManager.Instance != null)
            AudioManager.Instance.PlayWrongPin();
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnPinSet?.Invoke(row, column);

            if(connectPoints)
                ConnectToPreviousPoint(pinSocketPosition, pinType);
        }

        occupiedPinSocketPositionsDictionary.Add(pinSocketPosition, isRequiredPin);

        if(checkPins)
            CheckPins(true);
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

    private void ConnectToPreviousPoint(PinSocketPosition pinSocketPosition, PinType pinType)
    {
        RequiredSocket currentSocket = requiredSockets.Find(e => e.PinSocketPosition.Equals(pinSocketPosition) && e.PinType == pinType);

        if(currentSocket.Order > 0)
        {
            RequiredSocket previousSocket = requiredSockets.Find(e => e.PinType.Equals(pinType) && e.Order == currentSocket.Order - 1);
            ConnectTwoPoints(pinSocketPosition, previousSocket.PinSocketPosition, pinType);
        }
    }

    private void ConnectTwoPoints(PinSocketPosition currentPosition, PinSocketPosition previousPosition, PinType pinType)
    {
        GameObject thread = Instantiate(threadPrefab, container);
        thread.transform.localPosition = new Vector3(currentPosition.column * xSize + xSize * 0.5f, currentPosition.row * ySize + ySize * 0.5f);

        Vector2 dir = new Vector2((previousPosition.column - currentPosition.column) * xSize, -ySize * (previousPosition.row - currentPosition.row)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        thread.transform.localRotation = Quaternion.Euler(angle, 90, 0);

        float distance = Vector2.Distance(new Vector2(currentPosition.column * xSize, currentPosition.row * ySize), new Vector2(previousPosition.column * xSize, previousPosition.row * ySize));
        thread.transform.localScale = new Vector3(1000f, 1000f, -distance);
        ChangeColor(thread, pinType);
    }

    private void ChangeColor(GameObject gameObject, PinType pinType)
    {
        Color color;
        switch (pinType)
        {
            case PinType.Red:
                color = red;
                break;
            case PinType.Blue:
                color = blue;
                break;
            case PinType.Green:
                color = green;
                break;
            case PinType.Yellow:
                color = yellow;
                break;
            case PinType.Purple:
                color = purple;
                break;
            case PinType.Pink:
                color = pink;
                break;
            case PinType.Orange:
                color = orange;
                break;
            case PinType.White:
                color = white;
                break;
            default:
                color = new Color(1, 1, 1, 1);
                break;
        }
        
        
        if(gameObject.GetComponent<Pin>() == null)
        {
            var mat = gameObject.GetComponent<MeshRenderer>().material;
            mat.color = color;
            gameObject.GetComponent<MeshRenderer>().material = mat;
        }
        else
        {
            Material[] mat = gameObject.GetComponent<MeshRenderer>().materials;
            mat[1].color = color;
            gameObject.GetComponent<MeshRenderer>().materials = mat;
        }
    }

    private bool CheckPins(bool triggerCompleteAction)
    {
        if (occupiedPinSocketPositionsDictionary.Count != requiredSockets.Count)
            return false;

        bool chartCompleted = true;
        foreach (bool dictValue in occupiedPinSocketPositionsDictionary.Values)
        {
            if (dictValue == false)
            {
                chartCompleted = false;
                break;
            }
        }

        if (chartCompleted && triggerCompleteAction)
            onCompleteGrid();

        return chartCompleted;
    }

    public bool IsComplete()
    {
        return CheckPins(false);
    }


    public void ForceAddPin(GameObject pinPrefab, PinType pinType, int row, int column)
    {
        PinSocketPosition pinSocketPosition = new PinSocketPosition(column, row);
        GameObject pin = Instantiate(pinPrefab, pinSocketsDictionary[pinSocketPosition].transform.position, pinSocketsDictionary[pinSocketPosition].transform.rotation);
        pin.GetComponent<Pin>().SetPinType(pinType);
        ChangeColor(pin, pinType);
    }

    [Serializable]
    public class PinSocketPosition
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
    public class RequiredSocket
    {
        [SerializeField] private PinType pinType;
        [SerializeField] private PinSocketPosition pinSocketPosition;
        [SerializeField] private int order;

        public RequiredSocket(PinType pinType, PinSocketPosition pinSocketPosition, int order)
        {
            this.pinType = pinType;
            this.pinSocketPosition = pinSocketPosition;
            this.order = order;
        }

        public PinType PinType { get { return pinType; } }
        public PinSocketPosition PinSocketPosition { get { return pinSocketPosition; } }
        public int Order { get { return order; } }
    }
}
