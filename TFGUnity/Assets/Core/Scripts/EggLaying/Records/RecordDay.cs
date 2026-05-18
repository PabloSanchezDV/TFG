using UnityEngine;

[CreateAssetMenu(fileName = "RecordDay", menuName = "Scriptable Objects/RecordDay")]
public class RecordDay : ScriptableObject
{
    [SerializeField] private Date date;
    [SerializeField] private bool hasMale;
    [SerializeField] private BirdAnimation maleAnimation;
    [SerializeField] private bool hasFemale;
    [SerializeField] private BirdAnimation femaleAnimation;

    public Date Date { get { return date; } }
    public bool HasMale { get { return hasMale; } }
    public BirdAnimation MaleAnimation { get { return maleAnimation; } }
    public bool HasFemale { get { return hasFemale; } }
    public BirdAnimation FemaleAnimation { get {return femaleAnimation; } }
}
