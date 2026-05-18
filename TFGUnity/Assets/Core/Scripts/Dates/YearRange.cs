using System;
using UnityEngine;

[Serializable]
public class YearRange
{
    [SerializeField] private int minYear;
    [SerializeField] private int maxYear;

    public int MinYear { get { return minYear; } }
    public int MaxYear { get { return maxYear; } }

    public YearRange(int minYear, int maxYear)
    {
        this.minYear = minYear;
        this.maxYear = maxYear;
    }

    public int GetYearsBetween()
    {
        return maxYear - minYear + 1;
    }
}
