using System;
using UnityEngine;
using static Grid;

public class AnalysisState : FSMTemplateState
{
    GameManager gameManager;

    int eggsCount = 0;

    public AnalysisState(FSMTemplateMachine fsm) : base(fsm)
    {
        gameManager = (GameManager)fsm;
    }

    public override void Enter()
    {
        gameManager.OnEggRegistered.AddListener(PlayAnalysisStart);
    }

    public void PlayAnalysisStart()
    {
        gameManager.OnEggRegistered.RemoveListener(PlayAnalysisStart);
        AudioManager.Instance.PlayAnalysisIntro();
        gameManager.OnPaperBarGraphPickedUp.AddListener(IntroduceWhiteboard);
    }

    public void IntroduceWhiteboard()
    {
        gameManager.OnPaperBarGraphPickedUp.RemoveListener(IntroduceWhiteboard);
        AudioManager.Instance.PlayTendencyGraphPresentation();
        gameManager.OnPinSet.AddListener(AfterFirstMagnet);
    }

    public void AfterFirstMagnet(int row, int column)
    {
        gameManager.OnPinSet.RemoveListener(AfterFirstMagnet);
        AudioManager.Instance.PlayTendencyGraphFeedback();

        Action action = () =>
        {
            gameManager.RadarBird.Radar.TriggerAlarm();
            gameManager.RadarManager.ActivateRadar(gameManager.RadarBird);
        };

        gameManager.TriggerActionAfter(action, 20f);
        gameManager.OnTrendChartCompleted.AddListener(ExplainTrendChart);
    }

    public void ExplainTrendChart()
    {
        gameManager.OnTrendChartCompleted.RemoveListener(ExplainTrendChart);
        AudioManager.Instance.PlayTendencyGraphExplanation(gameManager.MigrationMap.IsMigrationCompleted());
        if (gameManager.MigrationMap.IsMigrationCompleted())
        {
            gameManager.OnPreviousMigrationRegistryPickedUp.AddListener(AddPreviousMapPins);
            gameManager.PreviousMigrationRegistry.interacted = false;
        }
        else 
            gameManager.OnMigrationMapCompleted.AddListener(FindPreviousMigrationRegistry);
    }

    public void FindPreviousMigrationRegistry()
    {
        gameManager.OnMigrationMapCompleted.RemoveListener(FindPreviousMigrationRegistry);
        AudioManager.Instance.PlayFindPreviousMigrationRegistry();
        gameManager.PreviousMigrationRegistry.interacted = false;
        gameManager.OnPreviousMigrationRegistryPickedUp.AddListener(AddPreviousMapPins);
    }

    public void AddPreviousMapPins()
    {
        gameManager.OnPreviousMigrationRegistryPickedUp.RemoveListener(AddPreviousMapPins);

        foreach (Route route in gameManager.PreviousMigrationsRoutes)
            gameManager.MigrationMap.AddRequiredRoute(route);

        AudioManager.Instance.PlayExplainPreviousMigrationRegistry();
        gameManager.OnPinSet.AddListener(GiveFeedbackAfterFirstPin);
    }

    public void GiveFeedbackAfterFirstPin(int row, int column)
    {
        gameManager.OnPinSet.RemoveListener(GiveFeedbackAfterFirstPin);
        AudioManager.Instance.PlayPreviousMigrationFeedback();
        gameManager.OnMigrationMapCompleted.AddListener(ExplainMigrationMap);
    }

    public void ExplainMigrationMap()
    {
        gameManager.OnMigrationMapCompleted.RemoveListener(ExplainMigrationMap);
        AudioManager.Instance.PlayExplainMigrationMap();
        gameManager.OnPrinterOn.AddListener(PrintGraphs);
    }

    public void PrintGraphs()
    {
        gameManager.OnPrinterOn.RemoveListener(PrintGraphs);
        gameManager.Printer.PrintGraphs();
        AudioManager.Instance.PlayExplainClimatologyGraphs();
        gameManager.OnChartFound.AddListener(ExplainClimateChange);
    }

    public void ExplainClimateChange()
    {
        gameManager.OnChartFound.RemoveListener(ExplainClimateChange);
        AudioManager.Instance.PlayExplainClimateChange();
        gameManager.OnProjectorTurnedOn.AddListener(ExplainClimateMaps);
    }

    public void ExplainClimateMaps()
    {
        gameManager.OnProjectorTurnedOn.RemoveListener(ExplainClimateMaps);
        AudioManager.Instance.PlayExplainClimateMaps();
    }
}
