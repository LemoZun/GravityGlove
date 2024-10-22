using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GloveEquiper : MonoBehaviour
{
    [SerializeField] GameObject glovePrefab;

    public void EquipGlove(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor directInteractor)
        {
            XRController controller = directInteractor.GetComponent<XRController>();
            if(controller != null)
            {
                controller.modelPrefab = glovePrefab.transform;
                //if (controller.model != null)
                //{
                //    Destroy(controller.model.gameObject);
                //}
                //controller.model = Instantiate(glovePrefab, controller.transform);

                Destroy(gameObject);
            }
        }
    }
}
