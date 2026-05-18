using System;
using UnityEngine;

[Serializable]
public class DateRange
{
    [SerializeField] private Date minDate;
    [SerializeField] private Date maxDate;

    private Date activeDate;

    public Date ActiveDate { get { return activeDate; } set { activeDate = value; CapDate(); } }

    public void Initialize()
    {
        activeDate = minDate;
    }

    public void CapDate()
    {
        if (activeDate.Month == maxDate.Month && activeDate.Day > maxDate.Day) // MaxDate
        {
            activeDate = minDate;
        }
        else if (activeDate.Month == minDate.Month && activeDate.Day < minDate.Day) // MinDate
        {
            activeDate = maxDate;
        }
        switch (activeDate.Month) // Months adjusment
        {
            case 1:
                CapMonth(31, 31);
                break;
            case 2:
                CapMonth(28, 31);
                break;
            case 3:
                CapMonth(31, 28);
                break;
            case 4:
                CapMonth(30, 31);
                break;
            case 5:
                CapMonth(31, 30);
                break;
            case 6:
                CapMonth(30, 31);
                break;
            case 7:
                CapMonth(31, 30);
                break;
            default:
                throw new System.Exception("Invalid Month value");
        }
    }

    private void CapMonth(int daysInMonth, int daysInPrevMonth)
    {
        if (activeDate.Day > daysInMonth)
        {
            activeDate.Day = 1;
            activeDate.Month++;
        }
        else if (activeDate.Day == 0)
        {
            activeDate.Day = daysInPrevMonth;
            activeDate.Month--;
        }
    }

    public int GetDaysBetween(bool limitInclusive)
    {
        DateTime min = new DateTime(2026, minDate.Month, minDate.Day);
        DateTime max = new DateTime(2026, maxDate.Month, maxDate.Day);


        if (limitInclusive)
            return (max - min).Days + 1;
        else
            return (max - min).Days;
    }

    public int GetDaysUntil(Date date)
    {
        DateTime givenDate = new DateTime(2026, date.Month, date.Day);
        DateTime min = new DateTime(2026, minDate.Month, minDate.Day);

        if (min > givenDate)
            throw new System.Exception("Given day goes before than min date");

        return (givenDate - min).Days;
    }
}
