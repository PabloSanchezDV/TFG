using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraShotManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private List<TransformValues> cameraValues;

    private Dictionary<CameraShot, TransformValues> cameraValuesDictionary;

    private void Awake()
    {
        cameraValuesDictionary = cameraValues.ToDictionary(e => e.cameraShot);
    }

    public void ChangeCameraShot(CameraShot cameraShot)
    {
        TransformValues values = cameraValuesDictionary[cameraShot];
        cameraTransform.localPosition = values.position;
        cameraTransform.localEulerAngles = values.rotation;
    }

    [Serializable]
    private class TransformValues
    {
        public CameraShot cameraShot;
        public Vector3 position;
        public Vector3 rotation;
    }
}
