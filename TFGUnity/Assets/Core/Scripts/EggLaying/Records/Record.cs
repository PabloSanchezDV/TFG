using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Record", menuName = "Scriptable Objects/Record")]
public class Record : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private Species species;
    [SerializeField] private Nest nest;
    [SerializeField] private CameraShot cameraShot;
    [SerializeField] private Date layingDate;
    [SerializeField] private List<RecordDay> recordDays;

    [SerializeField] int activeRecordDayIndex = 0;

    public Species Species { get { return species; } }
    public Nest Nest { get { return nest; } }
    public CameraShot CameraShot { get { return cameraShot; } }
    public bool HasEgg { get { return recordDays[activeRecordDayIndex].Date.IsAfterOrSameDate(layingDate); } }

    public RecordDay ActiveRecordDay { get { return recordDays[activeRecordDayIndex]; } }

    public Date ActiveDate { get { return recordDays[activeRecordDayIndex].Date; } }
    public Date LayingDate { get { return layingDate; } }

    public void NextDay()
    {
        activeRecordDayIndex++;

        if (activeRecordDayIndex >= recordDays.Count)
            activeRecordDayIndex = 0;

        GameManager.Instance.OnActiveRecordDayChanged?.Invoke(activeRecordDayIndex);
    }

    public void PrevDay()
    {
        activeRecordDayIndex--;

        if (activeRecordDayIndex < 0)
            activeRecordDayIndex = recordDays.Count - 1;

        GameManager.Instance.OnActiveRecordDayChanged?.Invoke(activeRecordDayIndex);
    }    
}
