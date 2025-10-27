using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject cellPrefab;

    public bool autoIterate = false;
    float timer = 0f;
    public float iterationInterval = 1.0f;

    bool gameStarted = false;
    bool simulationPaused = false;

    GameObject[,] gridObj;
    Cell[,] gridCell;
    GameProcess oneIter;
    ClickCellHandler clickCell;
    public Graphics graphics;
    GridGenerator gridGen;

    public event System.Action GameStartRequested;
    public event System.Action<bool> SimulationPaused;

    void Start()
    {
        StartScreenManager startScreen = GameObject.Find("StartScreenManager").GetComponent<StartScreenManager>();
        startScreen.LoadScreen(width, height);
    }

    public void RequestGameStart()
    {
        GameStartRequested?.Invoke();
        StartGame();
    }

    void StartGame()
    {
        gameStarted = true;
        simulationPaused = true;

        CameraSetup cameraSetup = FindObjectOfType<CameraSetup>();
        cameraSetup.Setup(width, height, Mathf.Max(width, height) * 1.15f);
        cameraSetup.gameStarted = true;

        GameObject gridGenObj = new GameObject("GridGenerator");
        gridGen = gridGenObj.AddComponent<GridGenerator>();
        gridGen.Initialize(cellPrefab, width, height);

        gridGen.GenerateGrid();
        gridObj = gridGen.GetGrid();

        oneIter = new GameProcess(width, height);
        gridCell = oneIter.GetGridCell();

        clickCell = new ClickCellHandler(gridCell);
        graphics.InitialSetup(gridObj, gridCell);

        graphics.SetRandomizeButtonCallback(OnRandomizeButtonClicked);
        graphics.SetSliderCallback(OnSpeedChanged);

        graphics.UpdateRandomizeButtonState(simulationPaused);
        graphics.UpdatePauseVisualization(simulationPaused);
        iterationInterval = 1.0f / graphics.GetSpeedValue();
    }

    void Update()
    {
        if (!gameStarted) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleSimulationPause();
        }

        if (simulationPaused)
        {
            clickCell.Process();
        }

        graphics.UpdateStates();
        if (simulationPaused)
        {
            graphics.UpdateSelection();
            return;
        }

        if (autoIterate)
        {
            timer += Time.deltaTime;

            if (timer >= iterationInterval)
            {
                timer = 0f;
                oneIter.Process();
            }
        }

        
        bool keyDown = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
        if (keyDown)
        {
            oneIter.Process();
        }
    }

    public void ToggleSimulationPause()
    {
        simulationPaused = !simulationPaused;
        SimulationPaused?.Invoke(simulationPaused);

        graphics.UpdatePauseVisualization(simulationPaused);
        graphics.UpdateRandomizeButtonState(simulationPaused);
    }

    public void SetSimulationPause(bool paused)
    {
        if (simulationPaused != paused)
        {
            simulationPaused = paused;
            SimulationPaused?.Invoke(simulationPaused);
            graphics.UpdatePauseVisualization(simulationPaused);
        }
    }

    public bool IsSimulationPaused()
    {
        return simulationPaused;
    }

    public void ForceSingleIteration()
    {
        if (gameStarted)
        {
            oneIter.Process();
            graphics.UpdateStates();
        }
    }

    void OnRandomizeButtonClicked()
    {
        if (simulationPaused)
        {
            gridGen.GenerateRandomStates(gridCell);
            graphics.UpdateStates();
        }
    }

    void OnSpeedChanged()
    {
        float newSpeed = graphics.GetSpeedValue();
        iterationInterval = 1.0f / newSpeed;
        graphics.UpdateSpeedText(newSpeed); 
        Debug.Log($"Speed changed to: {newSpeed:F1}x");
    }
}