using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MOTManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<GameObject> allBalls; // z. B. alle Kugeln
    [SerializeField] private int totalTrials = 5;
    [SerializeField] private float movementDuration = 5f;
    [SerializeField] private float speed = 1f; // Startgeschwindigkeit der Kugeln

    [Header("UI")]
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private MOTUIFlow uiFlow;

    private int currentTrial = 0;
    private int correctSelections = 0;
    private int targetIndex = 0;

    private float selectionStartTime;
    private List<float> reactionTimes = new List<float>();

    void Start()
    {
        resultCanvas.SetActive(false);
        //AssignRandomColors(); // Wird über UIFlow gestartet

        // Rigidbody-Setup sicherstellen
        foreach (var ball in allBalls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb == null)
                rb = ball.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = false;
        }
    }

    public void AssignRandomColors()
    {
        float radius = 2.5f;             // Radius des Spawnraums
        float minDistance = 0.6f;        // Mindestabstand zwischen Kugeln
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < allBalls.Count; i++)
        {
            allBalls[i].SetActive(false);

            Vector3 randomPos;
            int tries = 0;
            do
            {
                randomPos = transform.position + Random.insideUnitSphere * radius;
                randomPos.y = Mathf.Clamp(randomPos.y, 0.2f, 2.5f); // Optional: Höhenbegrenzung
                tries++;
            }
            while (!IsPositionValid(randomPos, usedPositions, minDistance) && tries < 100);

            usedPositions.Add(randomPos);
            allBalls[i].transform.position = randomPos;

            Rigidbody rb = allBalls[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }




        targetIndex = Random.Range(0, allBalls.Count);

        for (int i = 0; i < allBalls.Count; i++)
        {
            Renderer rend = allBalls[i].GetComponent<Renderer>();
            rend.material.color = (i == targetIndex) ? Color.green : Color.red;
        }

        StartCoroutine(MoveBalls());
    }

    private bool IsPositionValid(Vector3 newPos, List<Vector3> existingPositions, float minDistance)
    {
        foreach (var pos in existingPositions)
        {
            if (Vector3.Distance(newPos, pos) < minDistance)
                return false;
        }
        return true;
    }


    IEnumerator MoveBalls()
    {
        yield return new WaitForSeconds(2f); // Markierungszeit

        foreach (var ball in allBalls)
        {
            ball.SetActive(true);

            Vector3 dir = Random.onUnitSphere.normalized;
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(dir * speed, ForceMode.VelocityChange);

            Debug.Log($"Ball {ball.name} bewegt sich in Richtung {dir}");
        }

        yield return new WaitForSeconds(movementDuration);

        foreach (var ball in allBalls)
        {
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
