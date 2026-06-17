using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class NestManager : MonoBehaviour
{
    [SerializeField] private List<GOReference> nestReferencesList;
    private Dictionary<Nest, GameObject> nestDictionary;
    private Nest activeNest;

    private void Awake()
    {
        nestDictionary = nestReferencesList.ToDictionary(e => e.nest, e => e.nestGO);
    }

    public void ToggleNest(Nest nest)
    {
        GameObject go = nestDictionary[activeNest];
        if(go != null)
            go.SetActive(false);

        go = nestDictionary[nest];
        go.SetActive(true);

        activeNest = nest;
    }

    [Serializable]
    private class GOReference
    {
        public Nest nest;
        public GameObject nestGO;
    }
}
