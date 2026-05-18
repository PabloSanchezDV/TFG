using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EggRegistryManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<Species> availableSpeciesList;
    [SerializeField] private DateRange availableDates;
    [SerializeField] private List<Record> records;
    [SerializeField] private List<EggData> eggDataList;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI speciesText;
    [SerializeField] private TextMeshProUGUI dateText;

    private Dictionary<Record, bool> recordsDictionary;
    private int activeSpeciesIndex;

    private void Awake()
    {
        recordsDictionary = records.ToDictionary(e => e, e => false);
    }

    private void Start()
    {
        activeSpeciesIndex = 0;
        availableDates.Initialize();
        UpdateSpeciesText();
        UpdateDateText();
        NextDay();
        UpdateDateText();
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
        EggData data = new EggData(availableSpeciesList[activeSpeciesIndex], availableDates.ActiveDate);

        Record record = records.Find(e => e.Species == data.Species && e.LayingDate.Equals(data.Date));

        if (record != null && !recordsDictionary[record])
        {
            eggDataList.Add(data);
            recordsDictionary[record] = true;
            GraphsManager.Instance.UpdateGraph(data);
        }
        else
            Debug.Log("Provided EggData doesn't match any Record to be registered");
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
                return "Passer domesticus";
            case Species.BlueTit:
                return "Cyanistes caeruleus";
            case Species.GreatTit:
                return "Parus major";
            case Species.WhiteWagtail:
                return "Motacilla alba";
            case Species.BlackRedstart:
                return "Phoenicurus ochruros";
            case Species.TreeSparrow:
                return "Passer montanus";
            case Species.Shrike:
                return "Petronia petronia";
            case Species.CrestedTit:
                return "Lophophanes cristatus";
            case Species.MarshTit:
                return "Poecile palustris";
            case Species.CattleEgret:
                return "Motacilla flava";
            case Species.GreyWagtail:
                return "Motacilla cinerea";
            case Species.Redstart:
                return "Phoenicurus phoenicurus";
            case Species.Nightingale:
                return "Luscinia megarhynchos";
            default:
                throw new System.Exception("Invalid Species provided");
        }
    }
}
