using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    //XRController 
    [SerializeField] ActionBasedController controller;
    [SerializeField] Animator animator;
    private XRGrabInteractable selectedObject; // ���̷� ���� ��ü
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;
    private bool isPulled = false;

    //[SerializeField] Transform playerTransform;
    [SerializeField] float pullForce;
    //[SerializeField] float pullableRayDistance;

    private float originalZAngle; // ��Ʈ�ѷ��� ȸ������ �޾ƿ;���
    [SerializeField] float thresholdAngleVelocity; // �ӽð�

    Coroutine checkingFlickRoutine;

    public Animator controllerAnimator
    {
        get
        {
            if(animator != null)
                return animator;
            if (controller.model == null)
                return null;

            animator = controller.model.GetComponent<Animator>();
            return animator;
        }
    }
    private void Start()
    {
        //animator = controller.model.GetComponent<Animator>();
        //if (animator == null)
        //    Debug.LogError("�ִϸ����� ���� ����");

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
        //PlaySelectAnimation();

        //originalZAngle = controller.transform.rotation.eulerAngles.z;
        originalZAngle = controller.transform.localEulerAngles.z;
        StartChekingFlickRoutine();
    }

    public void SelectEnded()
    {
        Debug.Log("���� ��");
        //selectedObject = null;     
        //selectedObjectRb = null;


        isSelecting = false;
        isPulled = false;
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
        float unitSecond = 0.5f;
        float curZAngle;
        float deltaAngle;
        float angularVelocity;

        while (isSelecting && !isPulled)
        {
            //float curZAngle = controller.transform.rotation.eulerAngles.z;
            curZAngle = controller.transform.localEulerAngles.z;
            deltaAngle = Mathf.DeltaAngle(originalZAngle, curZAngle);
            angularVelocity = deltaAngle / unitSecond; //�ӽð�

            if (angularVelocity >= thresholdAngleVelocity)
            {
                //����� ����
                PullObject();
                StopChekingFlickRoutine();
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
        if (selectedObject != null && !isPulled)
        {

            //Debug.Log(selectedObject.name);

            selectedObjectRb.velocity = Vector3.zero;
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;
            
            // pullDirection.y = Mathf.Max(pullDirection.y, 0.1f); // ���������� ���ƿ��� �Ϸ��ߴµ� �̻��ϰ� ���ư�
            //Debug.Log(pullDirection.y);
            // ���ձ��� ������ + �������� �׸��� ���ƿ����� ������ �� �ʿ䰡 ����

            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.Impulse); //VelocityChange �غ�
            
            //������� ���� �� ������� �ȵ�
            isPulled = true;
        }
        else
        {
            Debug.Log("������Ʈ ����");
        }
    }


    public void PlaySelectAnimation()
    {
        
    }

    public void PlayGrabAnimation()
    {
        controllerAnimator.SetTrigger("Grab");
    }

    public void PlayUnGrabAnimation()
    {
        controllerAnimator.SetTrigger("UnGrab");
    }


}
