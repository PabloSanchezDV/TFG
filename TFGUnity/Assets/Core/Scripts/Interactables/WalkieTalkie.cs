using UnityEngine;

public class WalkieTalkie : MonoBehaviour
{
    bool interacted = false;

    public void PlayTutorialStart()
    {
        if(interacted) return;

        interacted = true;
        GameManager.Instance.OnWalkiePickedUp?.Invoke();
    }
}
