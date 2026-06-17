using UnityEngine;

public class TrendChartComparator : MonoBehaviour
{
    [SerializeField] private ChartType chartType;
    static bool interacted = false;

    public void CompareGraph()
    {
        if (interacted) return;

        if (chartType.Equals(ChartType.Temperature))
        {
            interacted = true;
            GameManager.Instance.OnChartFound?.Invoke();
        }
    }

    public enum ChartType { Temperature, Rain, Preassure, Cloudiness }
}
