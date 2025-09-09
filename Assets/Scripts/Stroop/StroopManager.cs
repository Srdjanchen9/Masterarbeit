using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StroopManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI stimulusText; 
    [SerializeField] private GameObject resultCanvas;      
    [SerializeField] private TextMeshProUGUI resultText;  
    [SerializeField] private StroopUIFlow uiFlow;

    [Header("Stimulus-Logik")]
    [SerializeField] private int totalTrials = 20;
    private int currentTrialIndex = 0;
    private List<Stimulus> stimulusList = new List<Stimulus>();
    private Stimulus currentStimulus;
    [SerializeField] private List<Transform> cubeSpawnPositions;
    [SerializeField] private GameObject redCube;
    [SerializeField] private GameObject greenCube;
    [SerializeField] private GameObject blueCube;
    [SerializeField] private float reactionTimeout = 5f;
    private bool hasAnswered = false;
    private Coroutine timeoutCoroutine;
    private bool inputAllowed = false;





    [Header("Leistungsdaten")]
    private int errorCount = 0;
    private float stimulusStartTime;
    private List<float> reactionTimes = new List<float>();


    private void Start()
    {
        resultCanvas.SetActive(false);
        GenerateStimuli();      // Erstellt Wort-Farb-Paare
        //ShowNextStimulus();     // Zeigt den ersten Stimulus an
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
            EndTask();
            return;
        }

        currentStimulus = stimulusList[currentTrialIndex];
        stimulusText.text = currentStimulus.word;
        stimulusText.color = currentStimulus.color;
        PlaceCubesRandomly();

        hasAnswered = false;
        inputAllowed = false; // Noch keine Eingabe erlaubt

        // Sicherer Delay, damit versehentliche Klicks vermieden werden
        StartCoroutine(EnableInputAfterDelay(0.9f));

        stimulusStartTime = Time.time;

        if (timeoutCoroutine != null)
            StopCoroutine(timeoutCoroutine);

        timeoutCoroutine = StartCoroutine(StartTimeoutWithDelay());
        
    }


    public void EvaluateAnswer(string selectedColor)
    {
        if (hasAnswered || !inputAllowed) return; // Eingabe ignorieren, falls zu früh
        hasAnswered = true;

        float reactionTime = Time.time - stimulusStartTime;
        reactionTimes.Add(reactionTime);

        string actualColor = currentStimulus.word.ToLower();
        bool isCorrect = selectedColor.ToLower() == actualColor;

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

    private void PlaceCubesRandomly()
    {
        // Kopie der Positionen, um Wiederholungen zu vermeiden
        List<Transform> availablePositions = new List<Transform>(cubeSpawnPositions);

        // Wähle zufällig Positionen für die 3 Würfel
        Transform redPos = GetRandomPosition(ref availablePositions);
        Transform greenPos = GetRandomPosition(ref availablePositions);
        Transform bluePos = GetRandomPosition(ref availablePositions);

        // Positioniere die Würfel
        redCube.transform.position = redPos.position;
        greenCube.transform.position = greenPos.position;
        blueCube.transform.position = bluePos.position;

        redCube.GetComponent<CubeFadeIn>().StartFade();
        greenCube.GetComponent<CubeFadeIn>().StartFade();
        blueCube.GetComponent<CubeFadeIn>().StartFade();

    }

    private Transform GetRandomPosition(ref List<Transform> positions)
    {
        int index = Random.Range(0, positions.Count);
        Transform selected = positions[index];
        positions.RemoveAt(index); // Stelle sicher, dass Position nicht doppelt vergeben wird
        return selected;
    }

    private IEnumerator ReactionTimeout()
    {
        yield return new WaitForSeconds(reactionTimeout);

        if (!hasAnswered)
        {
            reactionTimes.Add(reactionTimeout);
            errorCount++;
            currentTrialIndex++;
            ShowNextStimulus();
        }
    }

    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputAllowed = true;
    }

    public bool InputAllowed()
    {
        return inputAllowed;
    }




    private void EndTask()
    {
        float averageRT = 0f;
        if (reactionTimes.Count > 0)
        {
            float sum = 0f;
            foreach (float rt in reactionTimes)
                sum += rt;

            averageRT = sum / reactionTimes.Count;
        }

        stimulusText.text = "";
        resultCanvas.SetActive(true);
        resultText.text = $"Fehler: {errorCount} von {totalTrials}\nØ Reaktionszeit: {averageRT:F2} Sekunden";
        uiFlow.EndTask();
    }

    private IEnumerator StartTimeoutWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        timeoutCoroutine = StartCoroutine(ReactionTimeout());
    }


    public void StartStroopTask()
    {
        currentTrialIndex = 0;
        errorCount = 0;
        reactionTimes.Clear();
        ShowNextStimulus();
    }


    public void LoadNextScene()
    {
        SceneManager.LoadScene("GoNoGo");
    }
}
