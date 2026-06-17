using UnityEngine;

public class MigrationRegistry : MonoBehaviour
{
    [HideInInspector] public bool interacted = false;

    public void PlayTutorialStart()
    {
        if (interacted) return;

        interacted = true;
        GameManager.Instance.OnPreviousMigrationRegistryPickedUp?.Invoke();
    }
}
