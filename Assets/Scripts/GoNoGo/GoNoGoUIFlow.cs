using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoNoGoUIFlow : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject stimulusSphere;
    //[SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightrayInteractor;

    public void ShowControls()
    {
        instructionCanvas.SetActive(false);
        controlCanvas.SetActive(true);
    }

    public void StartTask()
    {
        controlCanvas.SetActive(false);
        stimulusSphere.SetActive(true);
        //leftHand.SetActive(false);
        rightrayInteractor.SetActive(false);

        // Starte ggf. Task-Logik hier
        // FindObjectOfType<GoNoGoManager>().BeginTask();
    }

    public void EndTask()
    {
        stimulusSphere.SetActive(false);
        rightrayInteractor.SetActive(true);
        //leftHand.SetActive(true);
        // Optional: Nächstes UI anzeigen
    }
}
