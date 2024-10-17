using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TransformController : MonoBehaviour
{
    /*
     * Track Position�� Track Rotation, Throw on Detach�� ���ָ� grab ������ �������� �ʵ��� �� �� �� ����
     * �� �Ʒ� �ڵ�� �ʿ䰡 ����..
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
        Debug.Log("Select ��");

        originalTransform = transform;
        XRRayInteractor interactor = args.interactorObject as XRRayInteractor;
        if (interactor is XRRayInteractor)
        {
            ControlTransform();
            isRaySelected = true;
        }
        else
        {
            Debug.Log("ray�� select�Ȱ��� �ƴ�");
        }
    }

    public void OnGrabEnded()
    {
        isRaySelected=false;
    }

    public void ControlTransform()
    {
        // ray�� ���������� ���� ��ġ�� ����
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
    }
}
