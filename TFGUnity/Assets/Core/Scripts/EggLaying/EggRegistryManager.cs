using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EggRegistryManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<Species> availableSpeciesList;
    [SerializeField] private DateRange availableDates;
    [SerializeField] private List<Record> registeredRecords;

    [Header("References")]
    [SerializeField] private RecordsManager recordsManager;
    [SerializeField] private TextMeshProUGUI speciesText;
    [SerializeField] private TextMeshProUGUI dateText;

    private int activeSpeciesIndex;

    private void Start()
    {
        activeSpeciesIndex = 0;
        availableDates.Initialize();
        UpdateSpeciesText();
        UpdateDateText();
        NextDay();
        UpdateDateText();

        foreach (var record in registeredRecords)
            GraphsManager.Instance.UpdateGraph(record.Species, record.LayingDate);
    }

    public void NextSpecies()
    {
        activeSpeciesIndex++;

        if (activeSpeciesIndex > availableSpeciesList.Count - 1)
            activeSpeciesIndex = 0;

        UpdateSpeciesText();
    }

    public void PrevSpecies()
    {
        activeSpeciesIndex--;

        if (activeSpeciesIndex < 0)
            activeSpeciesIndex = availableSpeciesList.Count - 1;

        UpdateSpeciesText();
    }

    public void NextDay()
    {
        availableDates.ActiveDate.Day++;
        availableDates.CapDate();
        UpdateDateText();
    }

    public void PrevDay()
    {
        availableDates.ActiveDate.Day--;
        availableDates.CapDate();
        UpdateDateText();
    }

    public void RegisterData()
    {
        Record record = recordsManager.ActiveRecord;

        if (!registeredRecords.Contains(record) && record.Species.Equals(availableSpeciesList[activeSpeciesIndex]) && record.LayingDate.Equals(availableDates.ActiveDate))
        {
            registeredRecords.Add(record);
            GraphsManager.Instance.UpdateGraph(availableSpeciesList[activeSpeciesIndex], availableDates.ActiveDate);
            GameManager.Instance.OnEggRegistered?.Invoke();
        }
        else
            AudioManager.Instance.PlayWrongRegistry();
    }

    private void UpdateSpeciesText()
    {
        speciesText.text = GetSpeciesName(availableSpeciesList[activeSpeciesIndex]);
    }

    private void UpdateDateText()
    {
        dateText.text = availableDates.ActiveDate.ToString();
    }

    private string GetSpeciesName(Species species)
    {
        switch (species)
        {
            case Species.HouseSparrow:
                return "Gorrión común";
            case Species.BlueTit:
                return "Herrerillo común";
            case Species.GreatTit:
                return "Carbonero común";
            case Species.WhiteWagtail:
                return "Lavandera blanca";
            case Species.BlackRedstart:
                return "Colirrojo tizón";
            case Species.TreeSparrow:
                return "Gorrión molinero";
            case Species.Shrike:
                return "Gorrión chillón";
            case Species.CrestedTit:
                return "Herrerillo capuchino";
            case Species.MarshTit:
                return "Carbonero palustre";
            case Species.CattleEgret:
                return "Lavandera boyera";
            case Species.GreyWagtail:
                return "Lavandera cascadeña";
            case Species.Redstart:
                return "Colirrojo real";
            case Species.Nightingale:
                return "Ruiseñor común";
            default:
                throw new System.Exception("Invalid Species provided");
        }
    }

    public bool IsRegistered(Record record)
    {
        return registeredRecords.Contains(record);
    }
}
