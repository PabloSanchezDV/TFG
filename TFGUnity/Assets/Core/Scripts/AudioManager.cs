using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource walkieAS;
    [SerializeField] private AudioSource radarAS;
    [SerializeField] private AudioSource printerAS;

    [Header("Registry Clips")]
    [SerializeField] private AudioClip wrongRegistryAC;

    [Header("Migration Clips")]
    [SerializeField] private AudioClip migrationAlarmAC;
    [SerializeField] private AudioClip wrongPinAC;

    [Header("Egg Laying Tutorial Clips")]
    [SerializeField] private AudioClip tutorialStartAC;
    [SerializeField] private AudioClip findDayAC;
    [SerializeField] private AudioClip birdAC;
    [SerializeField] private AudioClip registryAC;
    [SerializeField] private AudioClip barGraphAC;
    [SerializeField] private AudioClip eggLayingEndAC;

    [Header("Migration Tutorial Clips")]
    [SerializeField] private AudioClip migrationStartAC;
    [SerializeField] private AudioClip erosPresentationAC;
    [SerializeField] private AudioClip firstPinCompleteAC;
    [SerializeField] private AudioClip secondPinCompleteAC;
    [SerializeField] private AudioClip migrationEndAC;

    [Header("Analysis Clips")]
    [SerializeField] private AudioClip analysisStartAC;
    [SerializeField] private AudioClip tendencyGraphPresentationAC;
    [SerializeField] private AudioClip tendencyGraphFeedbackAC;
    [SerializeField] private AudioClip tendencyGraphExplanationAC;
    [SerializeField] private AudioClip tendencyGraphExplanationWithFindPreviousRegistryAC;
    [SerializeField] private AudioClip findPreviousMigrationRegistryAC;
    [SerializeField] private AudioClip explainPreviousMigrationRegistryAC;
    [SerializeField] private AudioClip previousMigrationFeedbackAC;
    [SerializeField] private AudioClip explainMigrationMapAC;
    [SerializeField] private AudioClip explainClimatologyGraphsAC;
    [SerializeField] private AudioClip explainClimateChangeAC;
    [SerializeField] private AudioClip explainClimateMapsAC;

    [Header("Other")]
    [SerializeField] private AudioClip printerOnAC;
    [SerializeField] private AudioClip printerOffAC;
    [SerializeField] private AudioClip printingAC;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    #region Egg Laying
    public void PlayTutorialStart()
    {
        walkieAS.clip = tutorialStartAC;
        walkieAS.Play();
    }

    public void PlayFindDayInCamera()
    {
        walkieAS.clip = findDayAC;
        walkieAS.Play();
    }

    public void PlayFindBird()
    {
        walkieAS.clip = birdAC;
        walkieAS.Play();
    }

    public void PlayRegistryTutorial()
    {
        walkieAS.clip = registryAC;
        walkieAS.Play();
    }

    public void PlayBarGraphTutorial()
    {
        walkieAS.clip = barGraphAC;
        walkieAS.Play();
    }

    public void PlayEggTutorialEndSound()
    {
        walkieAS.clip = eggLayingEndAC;
        walkieAS.Play();
    }
    #endregion

    #region Migration

    public void PlayMigrationAlarm()
    {
        radarAS.clip = migrationAlarmAC;
        radarAS.Play();
    }

    public void PlayWrongPin()
    {
        walkieAS.clip = wrongPinAC;
        walkieAS.Play();
    }

    public void PlayMigrationStart()
    {
        walkieAS.clip = migrationStartAC;
        walkieAS.Play();
    }

    public void PlayErosPresentation()
    {
        walkieAS.clip = erosPresentationAC;
        walkieAS.Play();
    }

    public void PlayFirstPinComplete()
    {
        walkieAS.clip = firstPinCompleteAC;
        walkieAS.Play();
    }

    public void PlaySecondPinComplete()
    {
        walkieAS.clip = secondPinCompleteAC;
        walkieAS.Play();
    }

    public void PlayMigrationEndSound()
    {
        walkieAS.clip = migrationEndAC;
        walkieAS.Play();
    }

    #endregion

    #region Analysis

    public void PlayAnalysisIntro()
    {
        walkieAS.clip = analysisStartAC;
        walkieAS.Play();
    }

    public void PlayTendencyGraphPresentation()
    {
        walkieAS.clip = tendencyGraphPresentationAC;
        walkieAS.Play();
    }

    public void PlayTendencyGraphFeedback()
    {
        walkieAS.clip = tendencyGraphFeedbackAC;
        walkieAS.Play();
    }

    public void PlayTendencyGraphExplanation(bool migrationCompleted)
    {
        if(migrationCompleted)
            walkieAS.clip = tendencyGraphExplanationWithFindPreviousRegistryAC;
        else    
            walkieAS.clip = tendencyGraphExplanationAC;
        walkieAS.Play();
    }

    public void PlayFindPreviousMigrationRegistry()
    {
        walkieAS.clip = findPreviousMigrationRegistryAC;
        walkieAS.Play();
    }

    public void PlayExplainPreviousMigrationRegistry()
    {
        walkieAS.clip = explainPreviousMigrationRegistryAC;
        walkieAS.Play();
    }

    public void PlayPreviousMigrationFeedback()
    {
        walkieAS.clip = previousMigrationFeedbackAC;
        walkieAS.Play();
    }

    public void PlayExplainMigrationMap()
    {
        walkieAS.clip = explainMigrationMapAC;
        walkieAS.Play();
    }

    public void PlayExplainClimatologyGraphs()
    {
        walkieAS.clip = explainClimatologyGraphsAC;
        walkieAS.Play();
    }

    public void PlayExplainClimateChange()
    {
        walkieAS.clip = explainClimateChangeAC;
        walkieAS.Play();
    }

    public void PlayExplainClimateMaps()
    {
        walkieAS.clip = explainClimateMapsAC;
        walkieAS.Play();
    }
    #endregion

    public void PlayWrongRegistry()
    {
        walkieAS.clip = wrongRegistryAC;
        walkieAS.Play();
    }

    public void PlayPrinterOn()
    {
        printerAS.clip = printerOnAC;
        printerAS.loop = false;
        printerAS.Play();
    }

    public void PlayPrinterOff()
    {
        printerAS.clip = printerOffAC;
        printerAS.loop = false;
        printerAS.Play();
    }

    public void PlayPrintingLoop()
    {
        printerAS.clip = printingAC;
        printerAS.loop = true;
        printerAS.Play();
    }

    public void StopPrintingLoop()
    {
        printerAS.loop = false;
        printerAS.Stop();
    }
}
