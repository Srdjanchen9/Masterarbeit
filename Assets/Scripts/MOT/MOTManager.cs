using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MOTManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<GameObject> allBalls;
    [SerializeField] private int totalTrials = 5;
    [SerializeField] private float initialSpeed = 1f;
    [SerializeField] private int initialBallCount = 8;
    [SerializeField] private int maxBallCount = 20;

    [Header("Difficulty Settings")]
    [SerializeField] private float speedIncrementPerTrial = 0.2f;
    [SerializeField] private int ballsIncrementPerTrial = 2;

    [Header("UI")]
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private MOTUIFlow uiFlow;

    private int currentTrial = 0;
    private int correctSelections = 0;
    private int targetIndex = 0;

    private float selectionStartTime;
    private List<float> reactionTimes = new List<float>();

    private float currentSpeed;
    private int currentBallCount;

    void Start()
    {
        resultCanvas.SetActive(false);

        currentSpeed = initialSpeed;
        currentBallCount = initialBallCount;

        foreach (var ball in allBalls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb == null) rb = ball.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            if (!ball.TryGetComponent<MOTBallPhysics>(out _))
                ball.AddComponent<MOTBallPhysics>();
        }
    }

    public void AssignRandomColors()
    {
        float radius = 2.5f;
        float minDistance = 0.6f;
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < allBalls.Count; i++)
            allBalls[i].SetActive(false);

        for (int i = 0; i < currentBallCount; i++)
        {
            Vector3 randomPos;
            int tries = 0;
            do
            {
                randomPos = transform.position + Random.insideUnitSphere * radius;
                randomPos.y = Mathf.Clamp(randomPos.y, 0.2f, 2.5f);
                tries++;
            }
            while (!IsPositionValid(randomPos, usedPositions, minDistance) && tries < 100);

            usedPositions.Add(randomPos);
            allBalls[i].transform.position = randomPos;

            Rigidbody rb = allBalls[i].GetComponent<Rigidbody>();
            var physics = allBalls[i].GetComponent<MOTBallPhysics>();
            physics.StopBall();

        }

        targetIndex = Random.Range(0, currentBallCount);

        for (int i = 0; i < currentBallCount; i++)
        {
            Renderer rend = allBalls[i].GetComponent<Renderer>();
            rend.material.color = (i == targetIndex) ? Color.green : Color.red;
        }

        StartCoroutine(MoveBalls());
    }

    private bool IsPositionValid(Vector3 newPos, List<Vector3> existingPositions, float minDistance)
    {
        foreach (var pos in existingPositions)
            if (Vector3.Distance(newPos, pos) < minDistance)
                return false;
        return true;
    }

    IEnumerator MoveBalls()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < currentBallCount; i++)
        {
            allBalls[i].SetActive(true);

            Vector3 dir = Random.onUnitSphere.normalized;
            var physics = allBalls[i].GetComponent<MOTBallPhysics>();
            physics.Launch(dir, currentSpeed);
        }

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < currentBallCount; i++)
        {
            allBalls[i].GetComponent<MOTBallPhysics>().StopBall();
            allBalls[i].GetComponent<Renderer>().material.color = Color.gray;
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
            // Schwierigkeit erhöhen
            currentSpeed += speedIncrementPerTrial;
            currentBallCount = Mathf.Min(currentBallCount + ballsIncrementPerTrial, maxBallCount);
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
        resultText.text = $"Du hast {correctSelections} von {totalTrials} korrekt erkannt!\n\u00d8 Reaktionszeit: {avgTime:F2} Sekunden";

        uiFlow.EndTask();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
