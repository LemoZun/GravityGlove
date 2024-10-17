using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    //private XRController controller;
    private XRGrabInteractable selectedObject; // ���̷� ���� ��ü
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
        Debug.Log("���� ����");
        selectedObject = args.interactableObject as XRGrabInteractable;
        selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
        isSelecting = true;
    }

    public void SelectEnded()
    {
        Debug.Log("���� ��");
        selectedObject = null;     
        selectedObjectRb = null;
        isSelecting = false;
    }


    public void PullObject()
    {
        Debug.Log("������Ʈ ���� ����");
        if (selectedObject != null && isSelecting)
        {
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;

            // ���ձ��� ������ + �������� �׸��� ���ƿ����� ������ �� �ʿ䰡 ����
            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("�ȵ�");
        }

    }
}
