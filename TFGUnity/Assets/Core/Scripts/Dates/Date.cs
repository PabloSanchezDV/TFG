using System;
using NUnit.Framework.Internal;
using UnityEngine;

[Serializable]
public class Date
{
    [SerializeField] private int day;
    [SerializeField] private int month;

    public int Day { get { return day; } set { day = value; } }
    public int Month { get { return month; } set { month = value; } }

    public Date(int day, int month)
    {
        this.day = day;
        this.month = month;
    }

    public bool IsAfterOrSameDate(Date date)
    {
        if(date.month != month)
            return month > date.month;

        return day >= date.day;
    }

    public override string ToString()
    {
        return day.ToString() + "/" + month.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Date other)
            return false;
        return day == other.day && month == other.month;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(day, month);
    }
}
