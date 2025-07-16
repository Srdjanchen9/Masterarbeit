using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MOTManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<GameObject> allBalls; // z. B. 3 Kugeln
    [SerializeField] private List<Transform> spawnPoints; // 3 Spawnpunkte
    [SerializeField] private int totalTrials = 5;
    [SerializeField] private float movementDuration = 5f;
    [SerializeField] private float speed = 1f; //Geschwindigkeit der Kugeln
    

    [Header("UI")]
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private MOTUIFlow uiFlow;

    private int currentTrial = 0;
    private int correctSelections = 0;
    private int targetIndex = 0;

    private float selectionStartTime;
    private List<float> reactionTimes = new List<float>();


    private Dictionary<GameObject, Vector3> moveDirs = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        resultCanvas.SetActive(false);
        //AssignRandomColors();
    }

    public void AssignRandomColors()
    {
        // Kugeln zufällig auf unterschiedliche Spawnpunkte setzen
        List<Transform> availableSpawns = new List<Transform>(spawnPoints);
        for (int i = 0; i < allBalls.Count; i++)
        {
            allBalls[i].SetActive(false);

            // Zufälligen SpawnPoint wählen und aus Liste entfernen (damit nicht doppelt)
            int randIndex = Random.Range(0, availableSpawns.Count);
            Transform chosenSpawn = availableSpawns[randIndex];
            availableSpawns.RemoveAt(randIndex);

            allBalls[i].transform.position = chosenSpawn.position;
        }

        // Zielindex zufällig wählen
        targetIndex = Random.Range(0, allBalls.Count);

        // Farben zuweisen
        for (int i = 0; i < allBalls.Count; i++)
        {
            Renderer rend = allBalls[i].GetComponent<Renderer>();
            rend.material.color = (i == targetIndex) ? Color.green : Color.red;
        }

        StartCoroutine(MoveBalls());
    }


    IEnumerator MoveBalls()
    {
        yield return new WaitForSeconds(2f); // Markierungszeit

        foreach (var ball in allBalls)
        {
            ball.SetActive(true);
            Vector3 dir = Random.onUnitSphere.normalized; // echte 3D-Richtung
            moveDirs[ball] = dir;
            Debug.Log($"Ball {ball.name} bewegt sich in Richtung {dir}");
        }

        float timer = 0f;
        while (timer < movementDuration)
        {
            foreach (var ball in allBalls)
            {
                Vector3 newPos = ball.transform.position + moveDirs[ball] * speed * Time.deltaTime;

                if (Vector3.Distance(Vector3.zero, newPos) > 2f)
                {
                    moveDirs[ball] = Vector3.Reflect(moveDirs[ball], Vector3.Normalize(newPos));
                }
                else
                {
                    ball.transform.position = newPos;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Optional: Material wieder neutral (z. B. grau)
        foreach (var ball in allBalls)
        {
            ball.GetComponent<Renderer>().material.color = Color.gray;
        }

        selectionStartTime = Time.time;

    }

    public void OnBallClickedIndex(int index)
    {
        float reactionTime = Time.time - selectionStartTime;
        reactionTimes.Add(reactionTime);


        if (index == targetIndex)
            correctSelections++;

        currentTrial++;

        if (currentTrial >= totalTrials)
        {
            EndTask();
        }
        else
        {
            AssignRandomColors();
        }
    }

    private void EndTask()
    {
        float avgTime = 0f;
        if (reactionTimes.Count > 0)
        {
            float sum = 0f;
            foreach (float t in reactionTimes) sum += t;
            avgTime = sum / reactionTimes.Count;
        }


        resultCanvas.SetActive(true);
        resultText.text = $"Du hast {correctSelections} von {totalTrials} korrekt erkannt!\nØ Reaktionszeit: {avgTime:F2} Sekunden";

        uiFlow.EndTask();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
