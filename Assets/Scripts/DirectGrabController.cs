using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectGrabController : MonoBehaviour
{
    //private XRDirectInteractor interactor;
    [SerializeField] XRGrabInteractable grabableObject;

    private void Start()
    {
        grabableObject = GetComponent<XRGrabInteractable>();
    }
    public void TrunOnTrack(SelectEnterEventArgs args)
    {
        if(args.interactor is XRDirectInteractor)
        {
            grabableObject.trackPosition = true;
            grabableObject.trackRotation = true;
        }
    }

    public void TrunOffTrack(SelectExitEventArgs args)
    {
        if (args.interactor is XRDirectInteractor)
        {
            grabableObject.trackPosition = false;
            grabableObject.trackRotation = false;
        }
    }
}
