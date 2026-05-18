using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{
    [SerializeField] XRRayInteractor teleportInteractor;
    [SerializeField] InputActionProperty teleportActivatorAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teleportInteractor.gameObject.SetActive(false);
        teleportActivatorAction.action.started += ActionStarted;
        teleportActivatorAction.action.canceled += ActionCanceled;
    }

    private void ActionStarted(InputAction.CallbackContext context)
    {
        teleportInteractor.gameObject.SetActive(true);
    }

    private void ActionCanceled(InputAction.CallbackContext context)
    {
        //teleportInteractor.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (teleportActivatorAction.action.WasReleasedThisFrame())
        {
            teleportInteractor.gameObject.SetActive(false);
        }
    }
}
