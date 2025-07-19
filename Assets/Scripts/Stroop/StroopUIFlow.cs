using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopUIFlow : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject redCube;
    [SerializeField] private GameObject blueCube;
    [SerializeField] private GameObject greenCube;
    [SerializeField] private GameObject stimulusSpawnPoints;
    [SerializeField] private GameObject stimulusCanvas;
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
        redCube.SetActive(true);
        blueCube.SetActive(true);
        greenCube.SetActive(true);
        stimulusSpawnPoints.SetActive(true);
        stimulusCanvas.SetActive(true);
        room.SetActive(true);  



        FindObjectOfType<StroopManager>().StartStroopTask();

    }

    public void EndTask()
    {
        redCube.SetActive(false);
        blueCube.SetActive(false);
        greenCube.SetActive(false);
        stimulusSpawnPoints.SetActive(false);
        stimulusCanvas.SetActive(false);
        room.SetActive(false);
    }
}
