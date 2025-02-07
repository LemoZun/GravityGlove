using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] private GameObject pointLight;
    [SerializeField] private InputActionReference primaryButton;
    private Light flash;
    private bool isFlashLightOn = true;

    private void OnEnable()
    {
        primaryButton.action.Enable();
    }

    private void OnDisable()
    {
        primaryButton.action.Disable();
    }

    private void Start()
    {
        flash = pointLight.GetComponent<Light>();
        flash.enabled = isFlashLightOn;
    }
    private void Update()
    {
        if(primaryButton.action.WasPressedThisFrame())
        {
            ToggleFlashLight();
        }
    }

    private void ToggleFlashLight()
    {
        isFlashLightOn = !isFlashLightOn;
        flash.enabled = isFlashLightOn;
    }
}
