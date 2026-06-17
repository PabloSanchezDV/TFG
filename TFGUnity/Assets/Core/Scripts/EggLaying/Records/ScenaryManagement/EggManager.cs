using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    [SerializeField] private GameObject blackRedstartEgg;
    [SerializeField] private GameObject defaultEgg;
    [SerializeField] private List<EggPosition> eggPositions;
    [SerializeField] private GameObject activeEgg;


    public void ToggleEgg(bool hasEgg, Species species, Nest nest)
    {
        GameObject eggGO;

        if (species.Equals(Species.BlackRedstart))
            eggGO = blackRedstartEgg;
        else
            eggGO = defaultEgg;

        if (activeEgg != null)
        {
            activeEgg.SetActive(false);
        }

        if (!hasEgg)
        {
            activeEgg = null;
            return;
        }

        activeEgg = eggGO;
        eggGO.SetActive(true);
        MoveEgg(eggGO, nest);
    }

    private void MoveEgg(GameObject egg, Nest nest)
    {
        foreach (EggPosition pos in eggPositions)
        {
            if(pos.Nest == nest)
            {
                egg.transform.localPosition = pos.Position;
                egg.transform.localEulerAngles = pos.Rotation;
                break;
            }
        }
    }

    [Serializable]
    private class EggPosition
    {
        [SerializeField] private Nest nest;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;

        public Nest Nest { get { return nest; } }
        public Vector3 Position { get { return position; } }
        public Vector3 Rotation { get { return rotation; } }
    }
}
