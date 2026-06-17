using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private GameObject[] birdPanels;
    int activeBirdPanel = 0;

    bool interacted = false;

    public void PlayRegistryTutorial()
    {
        if (interacted) return;

        interacted = true;
        GameManager.Instance.OnTabletPickedUp?.Invoke();
    }

    public void NextBirdPanel()
    {
        birdPanels[activeBirdPanel].SetActive(false);

        activeBirdPanel++;

        if (activeBirdPanel >= birdPanels.Length)
            activeBirdPanel = 0;

        birdPanels[activeBirdPanel].SetActive(true);
    }

    public void PrevBirdPanel()
    {
        birdPanels[activeBirdPanel].SetActive(false);

        activeBirdPanel--;

        if (activeBirdPanel < 0)
            activeBirdPanel = birdPanels.Length - 1;

        birdPanels[activeBirdPanel].SetActive(true);
    }
}
