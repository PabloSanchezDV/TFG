using UnityEngine;

public class EggLayingTutorialState : FSMTemplateState
{
    GameManager gameManager;

    int eggsCount = 0;

    public EggLayingTutorialState(FSMTemplateMachine fsm) : base(fsm)
    {
        gameManager = (GameManager)fsm;
    }

    public override void Enter()
    {
        gameManager.OnWalkiePickedUp.AddListener(StartTutorialAudio);
        gameManager.OnActiveRecordIndexChanged.AddListener(CheckCamera);
    }

    private void StartTutorialAudio()
    {
        gameManager.OnWalkiePickedUp.RemoveListener(StartTutorialAudio);
        AudioManager.Instance.PlayTutorialStart();
    }

    private void CheckCamera(int cameraIndex)
    {
        if(cameraIndex == 2)
        {
            gameManager.OnActiveRecordIndexChanged.RemoveListener(CheckCamera);
            AudioManager.Instance.PlayFindDayInCamera();
            gameManager.OnActiveRecordDayChanged.AddListener(CheckDay);
        }
    }

    private void CheckDay(int dayIndex)
    {
        if (dayIndex == 4)
        {
            gameManager.OnActiveRecordDayChanged.RemoveListener(CheckDay);
            AudioManager.Instance.PlayFindBird();
            gameManager.OnTabletPickedUp.AddListener(ExplainRegistry);
        }
    }

    private void ExplainRegistry()
    {
        gameManager.OnTabletPickedUp.RemoveListener(ExplainRegistry);
        AudioManager.Instance.PlayRegistryTutorial();
        gameManager.OnEggRegistered.AddListener(ExplainBarGraphs);
    }

    private void ExplainBarGraphs()
    {
        gameManager.OnEggRegistered.RemoveListener(ExplainBarGraphs);
        AudioManager.Instance.PlayBarGraphTutorial();
        gameManager.OnEggRegistered.AddListener(CountRegistry);
    }

    private void CountRegistry()
    {
        eggsCount++;

        if (eggsCount > 2)
            AudioManager.Instance.PlayEggTutorialEndSound();
    }
}
