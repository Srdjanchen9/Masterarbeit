using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOTUIFlow : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject controlCanvas;
    [SerializeField] private GameObject cubeOne;
    [SerializeField] private GameObject cubeTwo;
    [SerializeField] private GameObject cubeThree;
    [SerializeField] private GameObject cubeFour;
    [SerializeField] private GameObject cubeFive;
    [SerializeField] private GameObject cubeSix;
    [SerializeField] private GameObject cubeSeven;
    [SerializeField] private GameObject cubeEight;
    [SerializeField] private GameObject cubeNine;
    [SerializeField] private GameObject cubeTen;
    [SerializeField] private GameObject cubeEleven;
    [SerializeField] private GameObject cubeTwelve;
    [SerializeField] private GameObject cubeThirteen;
    [SerializeField] private GameObject cubeFourteen;
    [SerializeField] private GameObject cubeFifteen;
    [SerializeField] private GameObject cubeSixteen;
    [SerializeField] private GameObject cubeSeventeen;
    [SerializeField] private GameObject cubeEighteen;
    [SerializeField] private GameObject cubeNineteen;
    [SerializeField] private GameObject cubeTwenty;
    [SerializeField] private GameObject stimulusSpawnPoints;
    [SerializeField] private GameObject room;
    [SerializeField] private MOTManager motManager;

    public void ShowControls()
    {
        instructionCanvas.SetActive(false);
        controlCanvas.SetActive(true);
        cubeOne.SetActive(false);
        cubeTwo.SetActive(false);
        cubeThree.SetActive(false);
        cubeFour.SetActive(false);
        cubeFive.SetActive(false);
        cubeSix.SetActive(false);
        cubeSeven.SetActive(false);
        cubeEight.SetActive(false);
        cubeNine.SetActive(false);
        cubeTen.SetActive(false);
        cubeEleven.SetActive(false);
        cubeTwelve.SetActive(false);
        cubeThirteen.SetActive(false);
        cubeFourteen.SetActive(false);
        cubeFifteen.SetActive(false);
        cubeSixteen.SetActive(false);
        cubeSeventeen.SetActive(false);
        cubeEighteen.SetActive(false);
        cubeNineteen.SetActive(false);
        cubeTwenty.SetActive(false);
        stimulusSpawnPoints.SetActive(false);
        room.SetActive(false);
    }

    public void StartTask()
    {
        controlCanvas.SetActive(false);
        cubeOne.SetActive(true);
        cubeTwo.SetActive(true);
        cubeThree.SetActive(true);
        cubeFour.SetActive(true);
        cubeFive.SetActive(true);
        cubeSix.SetActive(true);
        cubeSeven.SetActive(true);
        cubeEight.SetActive(true);
        cubeNine.SetActive(true);
        cubeTen.SetActive(true);
        cubeEleven.SetActive(true);
        cubeTwelve.SetActive(true);
        cubeThirteen.SetActive(true);
        cubeFourteen.SetActive(true);
        cubeFifteen.SetActive(true);
        cubeSixteen.SetActive(true);
        cubeSeventeen.SetActive(true);
        cubeEighteen.SetActive(true);
        cubeNineteen.SetActive(true);
        cubeTwenty.SetActive(true);
        stimulusSpawnPoints.SetActive(true);
        room.SetActive(true);

        motManager.AssignRandomColors(); // Startet die Kugelbewegung
    }

    public void EndTask()
    {
        cubeOne.SetActive(false);
        cubeTwo.SetActive(false);
        cubeThree.SetActive(false);
        cubeFour.SetActive(false);
        cubeFive.SetActive(false);
        cubeSix.SetActive(false);
        cubeSeven.SetActive(false);
        cubeEight.SetActive(false);
        cubeNine.SetActive(false);
        cubeTen.SetActive(false);
        cubeEleven.SetActive(false);
        cubeTwelve.SetActive(false);
        cubeThirteen.SetActive(false);
        cubeFourteen.SetActive(false);
        cubeFifteen.SetActive(false);
        cubeSixteen.SetActive(false);
        cubeSeventeen.SetActive(false);
        cubeEighteen.SetActive(false);
        cubeNineteen.SetActive(false);
        cubeTwenty.SetActive(false);
        stimulusSpawnPoints.SetActive(false);
        room.SetActive(false);
    }

}
