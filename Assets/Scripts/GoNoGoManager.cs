using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GoNoGoManager : MonoBehaviour
{
    [SerializeField] private Image stimulusImage;
    [SerializeField] private Color goColor = Color.green;
    [SerializeField] private Color noGoColor = Color.red;
    [SerializeField] private float stimulusInterval = 1.0f;
    [SerializeField] private InputActionProperty triggerAction; // Action Reference

    private float timer = 0f;
    private bool isGoStimulus;

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
        ShowNextStimulus();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stimulusInterval)
        {
            ShowNextStimulus();
            timer = 0f;
        }

        if (triggerAction.action.WasPressedThisFrame())
        {
            if (isGoStimulus)
            {
                Debug.Log("Correct: Go stimulus pressed");
            }
            else
            {
                Debug.Log("Error: No-Go stimulus pressed");
            }
        }
    }

    void ShowNextStimulus()
    {
        isGoStimulus = (Random.value > 0.5f);
        stimulusImage.color = isGoStimulus ? goColor : noGoColor;
    }
}
