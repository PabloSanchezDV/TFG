using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PokeButton : MonoBehaviour
{
    [SerializeField] private Transform visualTarget;
    [SerializeField] private Vector3 localAxis;
    [SerializeField] private float resetSpeed;
    [SerializeField] private float followAngleThreshold;
    [SerializeField] private bool freeze;

    private Vector3 originaLocalPosition;
    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private bool isFollowing = false;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
        originaLocalPosition = visualTarget.localPosition;
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRDirectInteractor)
        {
            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));

            XRDirectInteractor interactor = (XRDirectInteractor)hover.interactorObject;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
            
            if (pokeAngle < followAngleThreshold)
            {
                isFollowing = true;
                freeze = false;
            }
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRDirectInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRDirectInteractor)
        {
            freeze = true;
        }
    }

    private void Update()
    {
        if (freeze)
            return;

        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, originaLocalPosition, Time.deltaTime * resetSpeed);
        }
    }
}
