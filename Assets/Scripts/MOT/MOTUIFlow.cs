using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOTUIFlow : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject firstCube;
    [SerializeField] private GameObject secondCube;
    [SerializeField] private GameObject thirdCube;
    [SerializeField] private GameObject fourthCube;
    [SerializeField] private GameObject fifthCube; 
    [SerializeField] private GameObject sixthCube;
    [SerializeField] private GameObject seventhCube;
    [SerializeField] private GameObject eighthCube;
    [SerializeField] private GameObject stimulusSpawnPoints;

    public void ShowControls()
    {
        instructionCanvas.SetActive(false);
        controlCanvas.SetActive(true);
    }

    public void StartTask()
    {
        Debug.Log("StartTask() aufgerufen");

        controlCanvas.SetActive(false);
        firstCube.SetActive(true);
        secondCube.SetActive(true);
        thirdCube.SetActive(true);
        fourthCube.SetActive(true);
        fifthCube.SetActive(true);  
        sixthCube.SetActive(true);
        seventhCube.SetActive(true);
        eighthCube.SetActive(true);
        stimulusSpawnPoints.SetActive(true);

        MOTManager.Instance.BeginTask();

    }

    public void EndTask()
    {
        firstCube.SetActive(false);
        secondCube.SetActive(false);
        thirdCube.SetActive(false);
        fourthCube.SetActive(false);
        fifthCube.SetActive(false);
        sixthCube.SetActive(false);
        seventhCube.SetActive(false);
        eighthCube.SetActive(false);
        stimulusSpawnPoints.SetActive(false);
    }
}
