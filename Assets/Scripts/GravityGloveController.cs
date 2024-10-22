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
    private XRGrabInteractable selectedObject; // 레이로 잡은 물체
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;
    private bool isPulled = false;

    //[SerializeField] Transform playerTransform;
    [SerializeField] float pullForce;
    //[SerializeField] float pullableRayDistance;

    private float originalZAngle; // 컨트롤러의 회전값을 받아와야함
    [SerializeField] float thresholdAngleVelocity; // 임시값

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
        //    Debug.LogError("애니메이터 참조 오류");

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
        Debug.Log("선택 시작");
        selectedObject = args.interactableObject as XRGrabInteractable;
        if (selectedObject == null)
        {
            Debug.Log("물체 없음");
            return;
        }
            
        selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
        if (selectedObjectRb == null)
            Debug.Log("오브젝트의 리지드바디 없음");
        isSelecting = true;
        //PlaySelectAnimation();

        //originalZAngle = controller.transform.rotation.eulerAngles.z;
        originalZAngle = controller.transform.localEulerAngles.z;
        StartChekingFlickRoutine();
    }

    public void SelectEnded()
    {
        Debug.Log("선택 끝");
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
            angularVelocity = deltaAngle / unitSecond; //임시값

            if (angularVelocity >= thresholdAngleVelocity)
            {
                //당겨짐 수행
                PullObject();
                StopChekingFlickRoutine();
                yield break;

            }
            else
            {
                Debug.Log("각속도 부족");
                // 지나치게 빨리 originalAngle을 업데이트해서 안당겨짐
                originalZAngle = curZAngle;
            }

            yield return delay;
        }
        
    }
    public void PullObject()
    {
        Debug.Log("오브젝트 당기기 시작");
        if (selectedObject != null && !isPulled)
        {

            //Debug.Log(selectedObject.name);

            selectedObjectRb.velocity = Vector3.zero;
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;
            
            // pullDirection.y = Mathf.Max(pullDirection.y, 0.1f); // 포물선으로 날아오게 하려했는데 이상하게 날아감
            //Debug.Log(pullDirection.y);
            // 눈앞까지 오도록 + 포물선을 그리며 날아오도록 조정해 줄 필요가 있음

            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.Impulse); //VelocityChange 해봄
            
            //당겨지는 도중 또 당겨지면 안됨
            isPulled = true;
        }
        else
        {
            Debug.Log("오브젝트 없음");
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
