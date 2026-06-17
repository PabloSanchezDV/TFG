using UnityEngine;

public class PaperBarGraph : MonoBehaviour
{
    static bool interacted = false;

    public void PlayTutorialStart()
    {
        if (interacted) return;

        interacted = true;
        GameManager.Instance.OnPaperBarGraphPickedUp?.Invoke();
    }
}
