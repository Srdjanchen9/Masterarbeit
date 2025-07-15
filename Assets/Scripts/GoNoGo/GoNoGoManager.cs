using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GoNoGoManager : MonoBehaviour
{
    [Header("Stimulus Settings")]
    [SerializeField] private Renderer stimulusRenderer;
    [SerializeField] private Color goColor = Color.green;
    [SerializeField] private Color noGoColor = Color.red;
    [SerializeField] private float stimulusInterval = 1.0f;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject stimulusObject; // Kugel selbst


    [Header("Input")]
    [SerializeField] private InputActionProperty triggerAction;

    [Header("End UI")]
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GoNoGoUIFlow uiFlow;


    [Header("Task Duration Settings")]
    [SerializeField] private float taskDuration = 60f; // Total Task Duration
    private float taskTimer = 0f;
    private bool taskIsRunning = true;

    private float stimulusTimer = 0f;
    private float stimulusStartTime;
    private bool isGoStimulus;

    private int correctResponses = 0;
    private int falseResponses = 0;
    private List<float> reactionTimes = new List<float>();

    private float difficultyIncreaseInterval = 15f;
    private float timeSinceLastIncrease = 0f;

    private void OnEnable()
    {
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Start()
    {
        stimulusRenderer.material.color = Color.black;
        ShowNextStimulus();
    }

    void Update()
    {
        if (!taskIsRunning)
            return;

        taskTimer += Time.deltaTime;
        timeSinceLastIncrease += Time.deltaTime;
        stimulusTimer += Time.deltaTime;

        // Steigere alle 15 Sek. die Schwierigkeit (Stimuli schneller)
        if (timeSinceLastIncrease >= difficultyIncreaseInterval)
        {
            stimulusInterval = Mathf.Max(0.3f, stimulusInterval - 0.2f);
            timeSinceLastIncrease = 0f;
        }

        if (stimulusTimer >= stimulusInterval)
        {
            ShowNextStimulus();
            stimulusTimer = 0f;
        }

        if (triggerAction.action.WasPressedThisFrame())
        {
            float reactionTime = Time.time - stimulusStartTime;

            if (isGoStimulus)
            {
                correctResponses++;
                reactionTimes.Add(reactionTime);
            }
            else
            {
                falseResponses++;
            }
        }

        if (taskTimer >= taskDuration)
        {
            taskIsRunning = false;
            EndTask();
        }
    }

    void ShowNextStimulus()
    {
        isGoStimulus = (Random.value > 0.5f);
        stimulusRenderer.material.color = isGoStimulus ? goColor : noGoColor;
        MoveStimulusToRandomPosition();
        stimulusStartTime = Time.time;
    }

    private void MoveStimulusToRandomPosition()
    {
        if (spawnPoints.Count == 0) return;

        int index = Random.Range(0, spawnPoints.Count);
        stimulusObject.transform.position = spawnPoints[index].position;
    }


    private void EndTask()
    {
        stimulusRenderer.material.color = Color.black;

        // Fehlerquote berechnen
        int totalResponses = correctResponses + falseResponses;
        float errorRate = totalResponses > 0 ? (falseResponses / (float)totalResponses) * 100f : 0f;

        // Durchschnittliche Reaktionszeit berechnen
        float avgRT = 0f;
        if (reactionTimes.Count > 0)
        {
            float sum = 0f;
            foreach (float rt in reactionTimes)
                sum += rt;
            avgRT = sum / reactionTimes.Count;
        }

        // UI anzeigen
        endCanvas.SetActive(true);
        resultText.text = $"Task beendet!\nFehlerquote: {errorRate:F1}%\nDurchschnittliche Reaktionszeit: {avgRT:F2}s";
        uiFlow.EndTask();

    }
    //public void LoadNextScene()
    //{
    // Erhöhe ggf. den Index oder gib expliziten Szenennamen an
    //SceneManager.LoadScene("MainMenu");
    //}

    public void LoadNextScene()
    {
        SceneManager.LoadScene("MOT");
    }

}
