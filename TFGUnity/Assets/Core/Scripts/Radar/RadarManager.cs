using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadarManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI memberDisplayText;
    [SerializeField] private MigrationMap migrationMap;

    [Header("Data")]
    [SerializeField] private List<Route> registeredRoutes;

    [Header("Radar Settings")]
    [SerializeField] private List<RadarBird> radarsList;
    [SerializeField] private float updateTimeInterval;
    [SerializeField] private bool canUpdateRadars;
    [SerializeField] private Bird activeRadar;
    [SerializeField] private bool activeRadarsSinceBeggining;

    public bool CanUpdateRadars { get { return canUpdateRadars; } set { canUpdateRadars = value; StartCoroutine(UpdateRadars()); } }

    private void Start()
    {
        foreach(RadarBird radarBird in radarsList)
            radarBird.Radar.Initialize(activeRadarsSinceBeggining, radarBird.Bird == activeRadar);

        migrationMap.UpdateStartingPins(registeredRoutes);

        ToggleRadar(activeRadar);

        StartCoroutine(UpdateRadars());
    }

    //public void NextRadar()
    //{
    //    activeRadarIndex++;

    //    if (activeRadarIndex >= radarsList.Count)
    //        activeRadarIndex = 0;

    //    ToggleRadar(radarsList[activeRadarIndex].Bird);
    //}

    //public void PrevRadar()
    //{
    //    activeRadarIndex--;

    //    if(activeRadarIndex < 0)
    //        activeRadarIndex = radarsList.Count - 1;

    //    ToggleRadar(radarsList[activeRadarIndex].Bird);
    //}

    public void ActivateRadar(RadarBird radarBird)
    {
        radarBird.Radar.CanMove = true;
    }

    public void ToggleRadar(Bird bird)
    { 
        RadarBird enabledRadar = radarsList.Find(e => e.Bird == activeRadar);

        if(enabledRadar == null)
            return;

        enabledRadar.Radar.DisableImage();
        enabledRadar = radarsList.Find(e => e.Bird == bird);
        enabledRadar.Radar.EnableImage();
        activeRadar = bird;

        UpdateText(activeRadar);
    }

    private void UpdateText(Bird bird)
    {
        string birdName;
        switch (bird)
        {
            case Bird.Persefone:
                birdName = "Perséfone";
                break;
            case Bird.Euridice:
                birdName = "Eurídice";
                break;
            default:
                birdName = bird.ToString();
                break;
        }
        memberDisplayText.text = birdName;
    }

    IEnumerator UpdateRadars()
    {
        if (canUpdateRadars)
        {
            foreach (RadarBird radarBird in radarsList)
                radarBird.Radar.UpdateMovement();

            yield return new WaitForSeconds(updateTimeInterval);

            StartCoroutine(UpdateRadars());
        }
    }

    [Serializable]
    public class RadarBird
    {
        [SerializeField] private Bird bird;
        [SerializeField] private Radar radar;

        public Bird Bird { get { return bird; } }
        public Radar Radar { get { return radar; } }
    }
}
