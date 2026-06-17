using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using static Grid;
using static RadarManager;

public class GameManager : FSMTemplateMachine
{
    public static GameManager Instance;

    [SerializeField] private Scene scene;
    [SerializeField] private RadarManager radarManager;
    [SerializeField] private RadarBird erosRadarBird;
    [SerializeField] private MigrationRegistry previousMigrationRegistry;
    [SerializeField] private MigrationMap migrationMap;
    [SerializeField] private List<Route> previousMigrationsRoutes;
    [SerializeField] private Printer printer; 

    //Events
    [HideInInspector] public UnityEvent OnWalkiePickedUp;
    [HideInInspector] public UnityEvent<int> OnActiveRecordIndexChanged;
    [HideInInspector] public UnityEvent<int> OnActiveRecordDayChanged;
    [HideInInspector] public UnityEvent OnTabletPickedUp;
    [HideInInspector] public UnityEvent OnEggRegistered;
    [HideInInspector] public UnityEvent<Bird> OnRadarSelected;
    [HideInInspector] public UnityEvent<int,int> OnPinSet;
    [HideInInspector] public UnityEvent OnPaperBarGraphPickedUp;
    [HideInInspector] public UnityEvent OnTrendChartCompleted;
    [HideInInspector] public UnityEvent OnMigrationMapCompleted;
    [HideInInspector] public UnityEvent OnPreviousMigrationRegistryPickedUp;
    [HideInInspector] public UnityEvent OnPrinterOn;
    [HideInInspector] public UnityEvent OnChartFound;
    [HideInInspector] public UnityEvent OnProjectorTurnedOn;

    //States
    public EggLayingTutorialState eggLayingTutorialState;
    public MigrationTutorialState migrationTutorialState;
    public AnalysisState analysisState;

    public RadarManager RadarManager { get { return radarManager; } }
    public RadarBird RadarBird { get { return erosRadarBird; } }
    public MigrationRegistry PreviousMigrationRegistry { get { return previousMigrationRegistry; } }
    public MigrationMap MigrationMap { get { return migrationMap; } }
    public List<Route> PreviousMigrationsRoutes { get { return previousMigrationsRoutes; } }
    public Printer Printer { get { return printer; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        eggLayingTutorialState = new EggLayingTutorialState(this);
        migrationTutorialState = new MigrationTutorialState(this);
        analysisState = new AnalysisState(this);
    }

    protected override void GetInitialState(out FSMTemplateState initialState)
    {
        switch (scene)
        {
            case Scene.EggLayingTutorial:
                initialState = eggLayingTutorialState;
                break;
            case Scene.MigrationTutorial:
                initialState = migrationTutorialState;
                break;
            case Scene.Analysis:
                initialState = analysisState;
                break;
            default:
                throw new System.Exception("Initial state of Game Manager couldn't be set");
        }
    }

    public void TriggerActionAfter(Action action, float time)
    {
        StartCoroutine(TriggerActionCoroutine(action, time));
    }

    IEnumerator TriggerActionCoroutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public enum Scene { EggLayingTutorial, MigrationTutorial, Analysis }
}
