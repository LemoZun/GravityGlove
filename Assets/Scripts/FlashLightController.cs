using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] GameObject pointLight;
    [SerializeField] InputActionReference primaryButton;
    private Light light;
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
        light = pointLight.GetComponent<Light>();
        light.enabled = isFlashLightOn;
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
        light.enabled = isFlashLightOn;
    }
}
