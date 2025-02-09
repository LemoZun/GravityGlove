using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectGrabController : MonoBehaviour
{
    //private XRDirectInteractor interactor;
    [FormerlySerializedAs("grabableObject")] [SerializeField] XRGrabInteractable grabbableObject;

    private void Start()
    {
        grabbableObject = GetComponent<XRGrabInteractable>();
    }
    public void TurnOnTrack(SelectEnterEventArgs args)
    {
        if (args.interactor is not XRDirectInteractor) 
            return;
        grabbableObject.trackPosition = true;
        grabbableObject.trackRotation = true;
    }

    public void TurnOffTrack(SelectExitEventArgs args)
    {
        if (args.interactor is not XRDirectInteractor) 
            return;
        grabbableObject.trackPosition = false;
        grabbableObject.trackRotation = false;
    }
}
