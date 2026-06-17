using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RecordsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EggRegistryManager eggRegistryManager;

    [Header("Data")]
    [SerializeField] private List<Record> records;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI camText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private GameObject registeredLabel;

    private SpeciesManager speciesManager;
    private CameraShotManager cameraShotManager;
    private NestManager nestManager;
    private EggManager eggManager;

    private int activeRecordIndex;
    public Record ActiveRecord { get { return records[activeRecordIndex]; } }

    private void Awake()
    {
        speciesManager = GetComponentInChildren<SpeciesManager>();
        cameraShotManager = GetComponentInChildren<CameraShotManager>();
        nestManager = GetComponentInChildren<NestManager>();
        eggManager = GetComponentInChildren<EggManager>();
    }

    private void Start()
    {
        GameManager.Instance.OnEggRegistered.AddListener(UpdateRegisteredLabel);
        activeRecordIndex = 0;
        UpdateRegisteredLabel();
        UpdateCameraText();
        UpdateDateText();
        UpdateScenary();
    }

    public void NextCamera()
    {
        activeRecordIndex++;

        if (activeRecordIndex > records.Count - 1)
            activeRecordIndex = 0;

        GameManager.Instance.OnActiveRecordIndexChanged?.Invoke(activeRecordIndex);

        UpdateCameraText();
        UpdateRegisteredLabel();
        UpdateDateText();
        UpdateScenary();
    }

    public void PrevCamera()
    {
        activeRecordIndex--;

        if (activeRecordIndex < 0)
            activeRecordIndex = records.Count - 1;

        GameManager.Instance.OnActiveRecordIndexChanged?.Invoke(activeRecordIndex);

        UpdateCameraText();
        UpdateRegisteredLabel();
        UpdateDateText();
        UpdateScenary();
    }

    public void NextDay()
    {
        records[activeRecordIndex].NextDay();
        UpdateDateText();
        UpdateScenary();
    }

    public void PrevDay()
    {
        records[activeRecordIndex].PrevDay();
        UpdateDateText();
        UpdateScenary();
    }

    private void UpdateCameraText()
    {
        camText.text = "Cam " + (activeRecordIndex + 1);
    }

    private void UpdateDateText()
    {
        dateText.text = "Fecha: " + records[activeRecordIndex].ActiveDate.ToString();
    }

    private void UpdateRegisteredLabel()
    {
        if(eggRegistryManager.IsRegistered(records[activeRecordIndex]))
            registeredLabel.SetActive(true);
        else
            registeredLabel.SetActive(false);
    }

    private void UpdateScenary()
    {
        Species sp = records[activeRecordIndex].Species;
        CameraShot shot = records[activeRecordIndex].CameraShot;
        Nest nest = records[activeRecordIndex].Nest;
        bool hasEgg = records[activeRecordIndex].HasEgg;
        RecordDay recordDay = records[activeRecordIndex].ActiveRecordDay;

        speciesManager.ToggleSpecies(sp, recordDay.HasMale, recordDay.HasFemale, recordDay.MaleAnimation, recordDay.FemaleAnimation);
        cameraShotManager.ChangeCameraShot(shot);
        nestManager.ToggleNest(nest);
        eggManager.ToggleEgg(hasEgg, sp, nest);
    }
}
