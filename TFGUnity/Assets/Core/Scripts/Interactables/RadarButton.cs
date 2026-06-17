using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadarButton : MonoBehaviour
{
    [SerializeField] private RadarManager radarManager;
    [SerializeField] private Bird bird;

    public void OnSelect(SelectEnterEventArgs args)
    {
        GameManager.Instance.OnRadarSelected?.Invoke(bird);
        radarManager.ToggleRadar(bird);
    }
}
