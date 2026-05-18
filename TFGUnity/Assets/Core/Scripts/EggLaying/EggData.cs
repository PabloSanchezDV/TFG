using System;
using UnityEngine;

[Serializable]
public class EggData
{
    [SerializeField] private Species species;
    [SerializeField] private Date date;

    public Species Species { get { return species; } }
    public Date Date { get { return date; } }

    public EggData(Species species, Date date)
    {
        this.species = species;
        this.date = date;
    }
}
