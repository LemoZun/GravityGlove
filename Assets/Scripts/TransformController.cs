using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TransformController : MonoBehaviour
{
    /*
     * Track Position과 Track Rotation, Throw on Detach를 꺼주면 grab 했을때 움직이지 않도록 해 줄 수 있음
     * 즉 아래 코드는 필요가 없다..
     */

    private Transform originalTransform;
    //private XRRayInteractor interactor;
    private bool isRaySelected = false;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }


    private void Update()
    {
        if (isRaySelected)
            ControlTransform();
    }

    public void OnGrabStarted(SelectEnterEventArgs args)
    {
        Debug.Log("Select 됨");

        originalTransform = transform;
        XRRayInteractor interactor = args.interactorObject as XRRayInteractor;
        if (interactor is XRRayInteractor)
        {
            ControlTransform();
            isRaySelected = true;
        }
        else
        {
            Debug.Log("ray로 select된것이 아님");
        }
    }

    public void OnGrabEnded()
    {
        isRaySelected=false;
    }

    public void ControlTransform()
    {
        // ray로 잡혔을때는 현재 위치를 고정
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
    }
}
