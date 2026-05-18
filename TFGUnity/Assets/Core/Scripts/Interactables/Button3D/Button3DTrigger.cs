using UnityEngine;

public class Button3DTrigger : MonoBehaviour
{
    [SerializeField] private Button3D button3D;
    [SerializeField] private Collider interactableCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.Equals(interactableCollider))
        {
            button3D.onPress?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(interactableCollider))
        {
            button3D.onRelease?.Invoke();
        }
    }
}
