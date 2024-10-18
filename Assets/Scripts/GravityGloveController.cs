using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    //private XRController controller;
    [SerializeField] GameObject controller;
    private XRGrabInteractable selectedObject; // ���̷� ���� ��ü
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;

    //[SerializeField] Transform playerTransform;
    [SerializeField] float pullForce;
    //[SerializeField] float pullableRayDistance;

    private float originalZAngle; // ��Ʈ�ѷ��� ȸ������ �޾ƿ;���
    [SerializeField] float thresholdAngleVelocity = 2.0f; // �ӽð�

    Coroutine checkingFlickRoutine;

    private void Start()
    {
        //playerTransform = transform;
    }

    private void OnEnable()
    {
        //XRInteractionManager
    }

    private void Update()
    {

    }

    public void SelectStarted(SelectEnterEventArgs args)
    {
        Debug.Log("���� ����");
        selectedObject = args.interactableObject as XRGrabInteractable;
        if (selectedObject == null)
        {
            Debug.Log("��ü ����");
            return;
        }
            
        selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
        if (selectedObjectRb == null)
            Debug.Log("������Ʈ�� ������ٵ� ����");
        isSelecting = true;
        originalZAngle = controller.transform.rotation.eulerAngles.z;

        StartChekingFlickRoutine();
    }

    public void SelectEnded()
    {
        Debug.Log("���� ��");
        selectedObject = null;     
        selectedObjectRb = null;
        isSelecting = false;

        StopChekingFlickRoutine();

    }

    private void StartChekingFlickRoutine()
    {
        if (checkingFlickRoutine == null)
            checkingFlickRoutine = StartCoroutine(CheckingFlickRoutine());
    }

    private void StopChekingFlickRoutine()
    {
        if (checkingFlickRoutine != null)
        {
            StopCoroutine(checkingFlickRoutine);
            checkingFlickRoutine = null;
        }

    }

    IEnumerator CheckingFlickRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        while (isSelecting)
        {
            float curZAngle = controller.transform.rotation.eulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(originalZAngle, curZAngle);
            float angularVelocity = deltaAngle / 0.5f; //�ӽð�

            if (angularVelocity >= thresholdAngleVelocity)
            {
                //����� ����
                PullObject();
                yield break;
            }
            else
            {
                Debug.Log("���ӵ� ����");
                // ����ġ�� ���� originalAngle�� ������Ʈ�ؼ� �ȴ����
                originalZAngle = curZAngle;
            }

            yield return delay;
        }
        
    }
    public void PullObject()
    {
        Debug.Log("������Ʈ ���� ����");
        if (selectedObject != null) //&& isSelecting)
        {
            Debug.Log(selectedObject.name);
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;

            // ���ձ��� ������ + �������� �׸��� ���ƿ����� ������ �� �ʿ䰡 ����
            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.VelocityChange); //Impulse�ε� �غ�
            Debug.Log(selectedObjectRb.velocity);
            //������� ���� �� ������� �ȵ�
            isSelecting = false;
        }
        else
        {
            Debug.Log("������Ʈ ����");
        }
    }
}
