using UnityEngine;

public class Projector : MonoBehaviour
{
    [SerializeField] private GameObject lightCone;
    [SerializeField] private MeshRenderer projectorMeshRenderer;
    [SerializeField] private Material projectorEarlyMaterial;
    [SerializeField] private Material projectorMaterial;
    [SerializeField] private GameObject[] slides;

    private int currentSlide;
    private bool turnedOn = false;
    private bool materialChanged = false;

    private void Start()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnChartFound.AddListener(ChangeEmissionTexture);

        projectorMeshRenderer.material = projectorEarlyMaterial;
        projectorMeshRenderer.materials[0].DisableKeyword("_EMISSION");
        lightCone.SetActive(false);
        ForceToggleSlides(false);
    }

    private void ChangeEmissionTexture()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnChartFound.RemoveListener(ChangeEmissionTexture);

        projectorMeshRenderer.material = projectorMaterial;
        materialChanged = true;
    }

    public void TogglePower()
    {
        if (turnedOn)
        {
            turnedOn = false;
            if (materialChanged)
            {
                lightCone.SetActive(false);
                ToggleSlides(false);
            }
            projectorMeshRenderer.materials[0].DisableKeyword("_EMISSION");

        }
        else
        {
            turnedOn = true;
            if (materialChanged)
            {
                projectorMaterial.EnableKeyword("_EMISSION");
                lightCone.SetActive(true);
                ToggleSlides(true);
            }
            projectorMeshRenderer.materials[0].EnableKeyword("_EMISSION");

            if (GameManager.Instance != null)
                GameManager.Instance.OnProjectorTurnedOn?.Invoke();
        }
    }

    public void NextSlide()
    {
        if (!turnedOn || !materialChanged)
            return;

        int targetSlide = currentSlide + 1;

        if (targetSlide > slides.Length - 1)
            targetSlide = 0;

        ChangeSlide(targetSlide);
    }

    public void PreviousSlide()
    {
        if (!turnedOn || !materialChanged)
            return;

        int targetSlide = currentSlide - 1;

        if (targetSlide < 0)
            targetSlide = slides.Length - 1;

        ChangeSlide(targetSlide);
    }

    private void ToggleSlides(bool activate)
    {
        if (slides == null || slides.Length == 0)
            return;

        if(activate)
            slides[currentSlide].SetActive(true);
        else 
            slides[currentSlide].SetActive(false);
    }

    private void ForceToggleSlides(bool activate)
    {
        if (slides == null || slides.Length == 0)
            return;

        foreach (GameObject slide in slides)
            slide.SetActive(activate);
    }

    private void ChangeSlide(int indexToActivate)
    {
        if (slides == null || slides.Length == 0)
            return;
     
        slides[currentSlide].SetActive(false);
        slides[indexToActivate].SetActive(true);
        currentSlide = indexToActivate;
    }
}
