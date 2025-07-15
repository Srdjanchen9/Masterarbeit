using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MOTManager : MonoBehaviour
{
    public static MOTManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private MOTUIFlow uiFlow;
    [SerializeField] private List<GameObject> allCubes; // 8 Kugeln (3 Zielobjekte + 5 Distraktoren)
    [SerializeField] private List<Transform> spawnPoints; // 8 Positionen im Raum
    [SerializeField] private int numberOfTargets = 3;
    [SerializeField] private float movementDuration = 8f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private int totalTrials = 5;

    private int currentTrial = 0;
    private int totalCorrectSelections = 0;
    private List<float> selectionTimes = new List<float>();
    private float selectionStartTime;

    [Header("Feedback & UI")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject resultCanvas;

    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> selectedObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> movementDirections = new Dictionary<GameObject, Vector3>();

    private Coroutine movementCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        resultCanvas.SetActive(false);
    }

    public void BeginTask()
    {
        Debug.Log("BeginTask gestartet.");

        selectedObjects.Clear();
        targets.Clear();
        movementDirections.Clear();

        uiFlow.StartTask();
        AssignSpawnPositions();
        MarkTargets();

        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(StartMovementPhase());

        selectionStartTime = Time.time;
    }

    private void AssignSpawnPositions()
    {
        List<Transform> availablePositions = new List<Transform>(spawnPoints);
        foreach (GameObject cube in allCubes)
        {
            int index = Random.Range(0, availablePositions.Count);
            cube.transform.position = availablePositions[index].position;
            availablePositions.RemoveAt(index);
        }
    }

    private void MarkTargets()
    {
        List<GameObject> availableCubes = new List<GameObject>(allCubes);
        for (int i = 0; i < numberOfTargets; i++)
        {
            int index = Random.Range(0, availableCubes.Count);
            GameObject target = availableCubes[index];
            availableCubes.RemoveAt(index);
            targets.Add(target);
            target.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private IEnumerator StartMovementPhase()
    {
        yield return new WaitForSeconds(2f); // kurze Markierungszeit

        foreach (GameObject cube in allCubes)
        {
            cube.GetComponent<Renderer>().material.color = Color.red;
            movementDirections[cube] = RandomDirection();
        }

        float timer = 0f;
        while (timer < movementDuration)
        {
            foreach (GameObject cube in allCubes)
            {
                Vector3 newPos = cube.transform.position + movementDirections[cube] * speed * Time.deltaTime;

                if (Vector3.Distance(Vector3.zero, newPos) > 2f)
                {
                    movementDirections[cube] = Vector3.Reflect(movementDirections[cube], Vector3.Normalize(newPos));
                }
                else
                {
                    cube.transform.position = newPos;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        ShowSelectionPhase();
    }

    private void ShowSelectionPhase()
    {
        foreach (GameObject cube in allCubes)
        {
            var selectable = cube.GetComponent<MOTSelectable>();
            if (selectable != null)
            {
                selectable.SetIsTarget(targets.Contains(cube));
            }
        }
    }

    public void RegisterSelection(MOTSelectable selected)
    {
        if (!selectedObjects.Contains(selected.gameObject))
            selectedObjects.Add(selected.gameObject);

        if (selectedObjects.Count >= numberOfTargets)
        {
            float trialTime = Time.time - selectionStartTime;
            selectionTimes.Add(trialTime);
            EvaluateSelection(selectedObjects);
        }
    }

    public void EvaluateSelection(List<GameObject> selected)
    {
        int correct = 0;
        foreach (GameObject obj in selected)
        {
            if (targets.Contains(obj)) correct++;
        }

        totalCorrectSelections += correct;
        selectedObjects.Clear();
        currentTrial++;

        if (currentTrial >= totalTrials)
        {
            EndTask();
        }
        else
        {
            BeginTask();
        }
    }

    private void EndTask()
    {
        float avgTime = 0f;
        if (selectionTimes.Count > 0)
        {
            float sum = 0f;
            foreach (float t in selectionTimes) sum += t;
            avgTime = sum / selectionTimes.Count;
        }

        resultCanvas.SetActive(true);
        resultText.text = $"Treffer insgesamt: {totalCorrectSelections} von {totalTrials * numberOfTargets}\nÿ Auswahlzeit: {avgTime:F2} s";
        uiFlow.EndTask();
    }

    private Vector3 RandomDirection()
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0f;
        return dir.normalized;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
