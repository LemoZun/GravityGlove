using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    private static readonly int Select = Animator.StringToHash("Select");
    private static readonly int UnSelect = Animator.StringToHash("UnSelect");
    private static readonly int Grab = Animator.StringToHash("Grab");
    private static readonly int UnGrab = Animator.StringToHash("UnGrab");

    //XRController 
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip pullingSound;

    private Animator handAnimator
    {
        get
        {
            if (animator != null)
                return animator;
            if (controller.model == null)
                return null;

            animator = controller.model.GetComponent<Animator>();
            return animator;
        }
    }

    [SerializeField] private XRDirectInteractor directInteractor;
    private Transform customAttachPoint;
    public Transform attachPoint
    {
        get
        {
            if(controller.model != null)
            {
                Debug.Log("컨트롤러의 모델 없음");
                return null;
            }

            if (customAttachPoint != null)
                return customAttachPoint;
            customAttachPoint = controller.model.Find("AttachPoint");

            if (customAttachPoint != null)
            {
                directInteractor.attachTransform = customAttachPoint;
                return directInteractor.attachTransform;
            }
            
            Debug.Log("customAttachPoint가 null");
            return customAttachPoint;
        }
    }

    private XRGrabInteractable selectedObject; // 레이로 잡은 물체
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;
    private bool isPulled = false;
    [SerializeField] private float pullForce;
    private float originalXAngle; // 컨트롤러의 회전값을 받아와야함
    [SerializeField] private float thresholdAngleVelocity; // 임시값
    private Coroutine checkingFlickRoutine;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            //audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogError("오디오 소스 없음");
        }
    }

    private void OnEnable()
    {
        //SetAttachPoint();
    }

    private void Update()
    {

    }

    public void SetAttachPoint()
    {
        if (controller.model == null)
        {
            Debug.Log("컨트롤러의 모델 없음");
            return;
        }

        if (customAttachPoint != null) 
            return;
        customAttachPoint = controller.model.Find("AttachPoint");

        if (customAttachPoint != null)
        {
            directInteractor.attachTransform = customAttachPoint;
        }
        else
        {
            Debug.Log("customAttachPoint가 null");
        }
        return;
    }

    public void SelectStarted(SelectEnterEventArgs args)
    {
        PlaySelectAnimation();
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
        originalXAngle = controller.transform.localEulerAngles.z;
        StartCheckingFlickRoutine();
    }

    public void SelectEnded()
    {
        Debug.Log("선택 끝");
        PlayUnSelectAnimation();
        //selectedObject = null;     
        //selectedObjectRb = null;


        isSelecting = false;
        isPulled = false;
        StopCheckingFlickRoutine();

    }

    private void StartCheckingFlickRoutine()
    {
        checkingFlickRoutine ??= StartCoroutine(CheckingFlickRoutine());
    }

    private void StopCheckingFlickRoutine()
    {
        if (checkingFlickRoutine == null) 
            return;
        StopCoroutine(checkingFlickRoutine);
        checkingFlickRoutine = null;

    }

    private IEnumerator CheckingFlickRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        float unitSecond = 0.5f;

        while (isSelecting && !isPulled)
        {
            float curZAngle = controller.transform.localEulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(originalXAngle, curZAngle);
            if (deltaAngle < 0)
            {
                originalXAngle = curZAngle;
                yield return delay;
                continue;
            }
                
            float angularVelocity = deltaAngle / unitSecond;

            if (angularVelocity >= thresholdAngleVelocity)
            {
                //당겨짐 수행
                PullObject();
                StopCheckingFlickRoutine();
                yield break;
            }
            Debug.Log("각속도 부족");
            originalXAngle = curZAngle;
            yield return delay;
        }
    }

    private void PullObject()
    {
        Debug.Log("오브젝트 당기기 시작");
        PlayPullingSound();
        if (selectedObject != null && !isPulled)
        {
            selectedObjectRb.velocity = Vector3.zero;
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;
            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.Impulse);
            //당겨지는 도중 또 당겨지면 안됨
            isPulled = true;
        }
        Debug.Log("오브젝트 없음");
        
    }

    private void PlayPullingSound()
    {
        if (audioSource == null || pullingSound == null) 
            return;
        audioSource.clip = pullingSound;
        audioSource.Play();
    }

    private void PlaySelectAnimation()
    {
        handAnimator.SetTrigger(Select);
    }

    private void PlayUnSelectAnimation()
    {
        handAnimator.SetTrigger(UnSelect);
    }

    public void PlayGrabAnimation()
    {
        handAnimator.SetTrigger(Grab);
    }

    public void PlayUnGrabAnimation()
    {
        handAnimator.SetTrigger(UnGrab);
    }


}
