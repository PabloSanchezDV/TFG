using UnityEngine;

public class MigrationTutorialState : FSMTemplateState
{
    GameManager gameManager;

    int pointsCount = 0;

    public MigrationTutorialState(FSMTemplateMachine fsm) : base(fsm)
    {
        gameManager = (GameManager)fsm;
    }

    public override void Enter()
    {
        gameManager.OnWalkiePickedUp.AddListener(StartTutorialAudio);
        gameManager.OnRadarSelected.AddListener(CheckRadarScreen);
    }

    public void StartTutorialAudio()
    {
        gameManager.RadarBird.Radar.TriggerAlarm();
        AudioManager.Instance.PlayMigrationStart();
    }

    public void CheckRadarScreen(Bird bird)
    {
        if(bird.Equals(Bird.Eros))
        {
            gameManager.OnRadarSelected.RemoveListener(CheckRadarScreen);
            AudioManager.Instance.PlayErosPresentation();
            gameManager.RadarManager.ActivateRadar(gameManager.RadarBird);
            gameManager.OnPinSet.AddListener(AfterFirstPinSet);
        }
    }

    public void AfterFirstPinSet(int row, int column)
    {
        if(row == 2 && column == 1)
        {
            gameManager.OnPinSet.RemoveListener(AfterFirstPinSet);
            AudioManager.Instance.PlayFirstPinComplete();
            gameManager.OnPinSet.AddListener(AfterSecondPinSet);
        }
    }

    public void AfterSecondPinSet(int row, int column)
    {
        if (row == 3 && column == 2)
        {
            gameManager.OnPinSet.RemoveListener(AfterSecondPinSet);
            AudioManager.Instance.PlaySecondPinComplete();
            gameManager.OnPinSet.AddListener(CountPins);
        }
    }

    public void CountPins(int i, int j)
    {
        pointsCount++;

        if (pointsCount > 2)
            AudioManager.Instance.PlayMigrationEndSound();
    }
}
