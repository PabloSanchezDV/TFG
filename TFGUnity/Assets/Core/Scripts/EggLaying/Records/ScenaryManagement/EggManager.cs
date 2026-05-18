using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    [SerializeField] private List<EggReference> eggReferences;
    private Dictionary<EggKeys, EggValues> eggsDictionary;

    private EggKeys activeEggKeys;

    private void Awake()
    {
        eggsDictionary = eggReferences.ToDictionary(e => e.keys, e => e.values);
    }

    public void ToggleEgg(bool hasEgg, Species species, Nest nest)
    {
        EggValues values;

        if (activeEggKeys != null)
        {
            values = eggsDictionary[activeEggKeys];
            values.gameObject.SetActive(false);
        }

        if(!hasEgg)
            return;

        EggKeys keys = new EggKeys(species, nest);
        values = eggsDictionary[keys];
        values.gameObject.SetActive(true);
        values.gameObject.transform.localPosition = values.position;
        values.gameObject.transform.localEulerAngles = values.rotation;
        activeEggKeys = keys;
    }

    [Serializable]
    private class EggReference
    {
        public EggKeys keys;
        public EggValues values;

    }

    [Serializable]
    private class EggValues
    {
        public GameObject gameObject;
        public Vector3 position;
        public Vector3 rotation;
    }

    [Serializable]
    private class EggKeys
    {
        public Species species;
        public Nest nest;

        public EggKeys(Species species, Nest nest)
        {
            this.species = species;
            this.nest = nest;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EggKeys other)
                return false;
            return species == other.species && nest == other.nest;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(species, nest);
        }
    }
}
