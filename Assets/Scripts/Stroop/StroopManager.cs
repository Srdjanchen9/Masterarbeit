using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StroopManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI stimulusText; // Das angezeigte Wort
    [SerializeField] private GameObject resultCanvas;      // Canvas mit Fehleranzahl + Button
    [SerializeField] private TextMeshProUGUI resultText;   // Der Text in diesem Canvas
    [SerializeField] private StroopUIFlow uiFlow;

    [Header("Stimulus-Logik")]
    [SerializeField] private int totalTrials = 20;
    private int currentTrialIndex = 0;
    private List<Stimulus> stimulusList = new List<Stimulus>();
    private Stimulus currentStimulus;

    [Header("Leistungsdaten")]
    private int errorCount = 0;

    private void Start()
    {
        resultCanvas.SetActive(false);
        GenerateStimuli();      // Erstellt Wort-Farb-Paare
        ShowNextStimulus();     // Zeigt den ersten Stimulus an
    }

    private void GenerateStimuli()
    {
        var colors = new List<Color> { Color.red, Color.green, Color.blue };
        var words = new List<string> { "Rot", "Grün", "Blau" };

        for (int i = 0; i < totalTrials; i++)
        {
            bool harder = i >= 5 && i % 5 == 0; // Schwierigkeit ab Trial 6, 11, 16 etc.
            string word;
            Color color;

            if (harder)
            {
                do
                {
                    word = words[Random.Range(0, words.Count)];
                    color = colors[Random.Range(0, colors.Count)];
                } while (word.ToLower() == ColorToName(color)); // nur inkongruente Trials
            }
            else
            {
                word = words[Random.Range(0, words.Count)];
                color = colors[Random.Range(0, colors.Count)];
            }

            stimulusList.Add(new Stimulus(word, color));
        }
    }

    private void ShowNextStimulus()
    {
        if (currentTrialIndex >= totalTrials)
        {
            EndTask(); // wenn alle durch → Endbildschirm
            return;
        }

        currentStimulus = stimulusList[currentTrialIndex];
        stimulusText.text = currentStimulus.word;
        stimulusText.color = currentStimulus.color;
    }

    public void EvaluateAnswer(string selectedColor)
    {
        string actualColor = ColorToName(currentStimulus.color);

        bool isCorrect = selectedColor.ToLower() == actualColor.ToLower();
        if (!isCorrect) errorCount++;

        currentTrialIndex++;
        ShowNextStimulus();
    }

    private string ColorToName(Color color)
    {
        if (color == Color.red) return "rot";
        if (color == Color.green) return "grün";
        if (color == Color.blue) return "blau";
        return "unbekannt";
    }

    private void EndTask()
    {
        stimulusText.text = "";
        resultCanvas.SetActive(true);
        resultText.text = $"Fehler: {errorCount} von {totalTrials}";
        uiFlow.EndTask();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("GoNoGo");
    }
}
