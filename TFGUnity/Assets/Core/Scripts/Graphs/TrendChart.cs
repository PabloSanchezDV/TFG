using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrendChart : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private DateRange dateRange;
    [SerializeField] private YearRange yearRange;
    [SerializeField] private Grid grid;

    private void Start()
    {
        grid.CreateGrid(yearRange.GetYearsBetween() + 1, dateRange.GetDaysBetween(true) + 1, ConnectPoints);
    }

    private void ConnectPoints()
    {
        Debug.Log("Conecting points...");
    }
}
