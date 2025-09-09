using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoNoGoUIFlow : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject stimulusSphere;
    //[SerializeField] private GameObject leftrayInteractor;
    [SerializeField] private GameObject rightrayInteractor;
    [SerializeField] private GameObject stimulusSpawnPoints;
    [SerializeField] private GameObject room;


    public void ShowControls()
    {
        instructionCanvas.SetActive(false);
        controlCanvas.SetActive(true);
        room.SetActive(false);
    }

    public void StartTask()
    {
        controlCanvas.SetActive(false);
        stimulusSphere.SetActive(true);
        //leftHand.SetActive(false);
        rightrayInteractor.SetActive(false);
        stimulusSpawnPoints.SetActive(true);
        room.SetActive(true);


        // Starte ggf. Task-Logik hier
        FindObjectOfType<GoNoGoManager>().BeginTask();
    }

    public void EndTask()
    {
        stimulusSphere.SetActive(false);
        stimulusSpawnPoints.SetActive(false);
        rightrayInteractor.SetActive(true);
        room.SetActive(false);
        //leftHand.SetActive(true);
        
    }
}
