using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class GraphsManager : MonoBehaviour
{
    public static GraphsManager Instance;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI speciesText;

    [Header("Graphs")]
    [SerializeField] private BarGraph houseSparrowGraph;
    [SerializeField] private BarGraph blueTitGraph;
    [SerializeField] private BarGraph greatTitGraph;
    [SerializeField] private BarGraph whiteWagtailGraph;
    [SerializeField] private BarGraph blackRedstartGraph;

    Dictionary<Species, BarGraph> barGraphsDictionary;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        barGraphsDictionary = new Dictionary<Species, BarGraph>();

        houseSparrowGraph.CreateGraph();
        blueTitGraph.CreateGraph();
        greatTitGraph.CreateGraph();
        whiteWagtailGraph.CreateGraph();
        blackRedstartGraph.CreateGraph();

        barGraphsDictionary.Add(Species.HouseSparrow, houseSparrowGraph);
        barGraphsDictionary.Add(Species.BlueTit, blueTitGraph);
        barGraphsDictionary.Add(Species.GreatTit, greatTitGraph);
        barGraphsDictionary.Add(Species.WhiteWagtail, whiteWagtailGraph);
        barGraphsDictionary.Add(Species.BlackRedstart, blackRedstartGraph);
    }

    //public void UpdateText(Species species)
    //{
    //    switch (species)
    //    {
    //        case Species.HouseSparrow:
    //            speciesText.text = "Gorrión común";
    //            break;
    //        case Species.BlueTit:
    //            speciesText.text = "Herrerillo común";
    //            break;
    //        case Species.GreatTit:
    //            speciesText.text = "Carbonero común";
    //            break;
    //        case Species.WhiteWagtail:
    //            speciesText.text = "Lavandera blanca";
    //            break;
    //        case Species.BlackRedstart:
    //            speciesText.text = "Colirrojo tizón";
    //            break;
    //        default:
    //            throw new System.Exception($"Unable to find name for {species}.");
    //    }
    //}

    //public void AddSpeciesGraph(Species species)
    //{
    //    BarGraph graphToAdd = null;

    //    switch (species)
    //    {
    //        case Species.BlueTit:
    //            graphToAdd = blueTitGraph;
    //            break;
    //        case Species.GreatTit:
    //            graphToAdd = greatTitGraph;
    //            break;
    //        case Species.WhiteWagtail:
    //            graphToAdd = whiteWagtailGraph;
    //            break;
    //        case Species.BlackRedstart:
    //            graphToAdd = blackRedstartGraph;
    //            break;
    //        default:
    //            throw new System.Exception("Provided Species doesn't seem to have a Graph associated");
    //    }

    //    graphToAdd.CreateGraph();
    //    barGraphsDictionary.Add(species, graphToAdd);
    //}

    public void UpdateGraph(Species species, Date date)
    {
        BarGraph graph = barGraphsDictionary[species];
        if(!graph.gameObject.activeInHierarchy)
            graph.gameObject.SetActive(true);
        graph.UpdateGraph(date);
    }
}
