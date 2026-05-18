using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class SpeciesManager : MonoBehaviour
{
    [SerializeField] private List<CoupleGOReference> coupleReferencesList;

    private Dictionary<Species, CoupleGOReference> speciesDictionary;

    Species activeSpecies;

    private void Awake()
    {
        speciesDictionary = coupleReferencesList.ToDictionary(e => e.species);
    }

    public void ToggleSpecies(Species species, bool hasMale, bool hasFemale, BirdAnimation maleAnimation, BirdAnimation femaleAnimation)
    {
        CoupleGOReference reference = speciesDictionary[activeSpecies];

        reference.male.SetActive(false);
        reference.female.SetActive(false);

        reference = speciesDictionary[species];

        reference.male.SetActive(hasMale);
        reference.female.SetActive(hasFemale);

        if(hasMale)
            ToggleAnimation(reference.male, maleAnimation);
        
        if(hasFemale)
            ToggleAnimation(reference.female, femaleAnimation);
        
        activeSpecies = species;
    }

    private void ToggleAnimation(GameObject bird, BirdAnimation animation)
    {
        Animator anim = bird.GetComponent<Animator>();
        
        if(anim != null)
            anim.SetTrigger(animation.ToString());
    }

    [Serializable]
    private class CoupleGOReference
    {
        public Species species;
        public GameObject male;
        public GameObject female;
    }
}
