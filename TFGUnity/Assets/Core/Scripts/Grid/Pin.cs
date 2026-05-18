using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private PinType pinType;

    public PinType PinType {  get { return pinType; } }
}
