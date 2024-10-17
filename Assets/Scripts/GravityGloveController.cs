using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    //private XRController controller;
    private XRGrabInteractable selectedObject; // 레이로 잡은 물체
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;

    //[SerializeField] Transform playerTransform;
    [SerializeField] float pullForce;
    [SerializeField] float pullableRayDistance;

    private void Start()
    {
        //playerTransform = transform;
    }

    private void OnEnable()
    {
        //XRInteractionManager
    }

    public void SelectStarted(SelectEnterEventArgs args)
    {
        Debug.Log("선택 시작");
        selectedObject = args.interactableObject as XRGrabInteractable;
        selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
        isSelecting = true;
    }

    public void SelectEnded()
    {
        Debug.Log("선택 끝");
        selectedObject = null;     
        selectedObjectRb = null;
        isSelecting = false;
    }


    public void PullObject()
    {
        Debug.Log("오브젝트 당기기 시작");
        if (selectedObject != null && isSelecting)
        {
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;

            // 눈앞까지 오도록 + 포물선을 그리며 날아오도록 조정해 줄 필요가 있음
            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("안됨");
        }

    }
}
