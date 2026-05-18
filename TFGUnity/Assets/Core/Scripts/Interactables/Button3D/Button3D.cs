using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    public UnityEvent onPress = new UnityEvent();
    public UnityEvent onRelease = new UnityEvent();

    Animator anim;
    bool pressed = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finger"))
        {
            anim.SetTrigger("Trigger");

            if (!pressed)
            {
                onPress?.Invoke();
                pressed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            anim.SetTrigger("Trigger");

            if (pressed)
            {
                onRelease?.Invoke();
                pressed = false;
            }
        }
    }
}
