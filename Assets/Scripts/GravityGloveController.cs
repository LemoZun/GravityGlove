using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GravityGloveController : MonoBehaviour
{
    //private XRController controller;
    [SerializeField] GameObject controller;
    private XRGrabInteractable selectedObject; // 레이로 잡은 물체
    private Rigidbody selectedObjectRb;
    private bool isSelecting = false;

    //[SerializeField] Transform playerTransform;
    [SerializeField] float pullForce;
    //[SerializeField] float pullableRayDistance;

    private float originalZAngle; // 컨트롤러의 회전값을 받아와야함
    [SerializeField] float thresholdAngleVelocity = 2.0f; // 임시값

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
        originalZAngle = controller.transform.rotation.eulerAngles.z;

        StartChekingFlickRoutine();
    }

    public void SelectEnded()
    {
        Debug.Log("선택 끝");
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
            float angularVelocity = deltaAngle / 0.5f; //임시값

            if (angularVelocity >= thresholdAngleVelocity)
            {
                //당겨짐 수행
                PullObject();
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
        if (selectedObject != null) //&& isSelecting)
        {
            Debug.Log(selectedObject.name);
            Vector3 pullDirection = (transform.position - selectedObject.transform.position).normalized;

            // 눈앞까지 오도록 + 포물선을 그리며 날아오도록 조정해 줄 필요가 있음
            selectedObjectRb.AddForce(pullDirection * pullForce, ForceMode.VelocityChange); //Impulse로도 해봄
            Debug.Log(selectedObjectRb.velocity);
            //당겨지는 도중 또 당겨지면 안됨
            isSelecting = false;
        }
        else
        {
            Debug.Log("오브젝트 없음");
        }
    }
}
