using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI;

public class PinSocket : MonoBehaviour
{
    private int row;
    private int column;
    public Action<PinType, int, int> onOccupyAction;
    public Action<int, int> onDeoccupyAction;

    public int Row { get { return row; } set { row = value; } }
    public int Column { get { return column; } set { column = value; } }

    public void OccupySocket(SelectEnterEventArgs args)
    {
        GameObject pin = args.interactableObject.transform.gameObject;
        onOccupyAction(pin.GetComponent<Pin>().PinType, row, column);
    }

    public void UnoccupySocket()
    {
        onDeoccupyAction(row, column);
    }
}
