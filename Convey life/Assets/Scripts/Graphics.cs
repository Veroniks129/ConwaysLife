using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Graphics : MonoBehaviour
{
    public Dictionary<string, string> colorsHex = new Dictionary<string, string>()
    {
        {"alive", "E0E1FF"},
        {"dead", "0F004F"},
        {"selectedA", "BFBDF2"},
        {"selectedD", "4F478F"},
        {"border", "08002B"},
        {"rButtonActive", "07736E"},
        {"rButtonInactive", "89AEAC"}
    };

    public Dictionary<string, Color> colors;

    private GameObject[,] gridObj;
    private Cell[,] gridCell;
    private bool isPaused = false;

    private GameObject pauseTextObj;
    private Text pauseText;
    private GameObject backgroundObj;

    public GameObject randomizeButton;
    private Button buttonComp;
    private Image buttonImage;

    //private GameObject speedInputFieldObj;
    //private InputField speedInputField;
    //private Text speedPlaceholderText;
    //private Text speedInputText;

    public GameObject sliderObj;
    Slider slider;

    public GameObject speedTextObj;
    TextMeshProUGUI speedText;

    public GameObject sliderBack;
    public GameObject speedMenu;

    private int gridWidth;
    private int gridHeight;

    public void InitialSetup(GameObject[,] gridObj, Cell[,] gridCell)
    {
        this.gridObj = gridObj;
        this.gridCell = gridCell;
        this.gridWidth = gridCell.GetLength(0);
        this.gridHeight = gridCell.GetLength(1);

        colors = new Dictionary<string, Color>();
        foreach (string key in colorsHex.Keys)
        {
            colors[key] = HexToColor(colorsHex[key]);
        }

        randomizeButton.SetActive(true);
        sliderObj.SetActive(true);
        speedTextObj.SetActive(true);
        sliderBack.SetActive(true);
        speedMenu.SetActive(true);

        speedText = speedTextObj.GetComponent<TextMeshProUGUI>();
        slider = sliderObj.GetComponent<Slider>();
        buttonComp = randomizeButton.GetComponent<Button>();
        buttonImage = randomizeButton.GetComponent<Image>();

        CreatePauseText();

        slider.value = 0.2f;
        UpdateSpeedText(1.0f);

        SetGUIPosition();
    }

    void CreatePauseText()
    {
        GameObject canvasObj = new GameObject("UICanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        pauseTextObj = new GameObject("PauseText");
        pauseTextObj.transform.SetParent(canvasObj.transform);

        pauseText = pauseTextObj.AddComponent<Text>();
        pauseText.text = "PAUSED";

        pauseText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        pauseText.fontSize = 24;
        pauseText.color = Color.white;
        pauseText.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = pauseTextObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(200, 50);

        pauseTextObj.SetActive(false);
    }

    void SetGUIPosition()
    {
        const int objCnt = 3;
        float leftMenuBias = 0.05f;
        Vector2[] positions = new Vector2[objCnt]
        {
            new Vector2(leftMenuBias, 0.8f), //randomize
            new Vector2(leftMenuBias, 0.65f), // speed menu
            new Vector2(0.5f, 1f), //pause text
        };

        Vector2[] biases = new Vector2[objCnt]
        {
            new Vector2(-12, 0),
            new Vector2(0, 0),
            new Vector2(-1f, 0),
        };

        GameObject[] objects = new GameObject[objCnt]
        {
            randomizeButton,
            speedMenu,
            pauseTextObj
        };

        for (int i = 0; i < objCnt; i++)
        {
            SetAnchoredPosition(objects[i], positions[i], biases[i]);
        }
    }

    void SetAnchoredPosition(GameObject obj, Vector2 pos, Vector2 delta)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = pos;
        rt.anchorMax = pos;
        rt.pivot = pos;
        rt.anchoredPosition = delta;
    }

    public void UpdateSpeedText(float value)
    {
        speedText.text = $"Speed: {value:F1} it/s";
    }

    public void UpdatePauseVisualization(bool paused)
    {
        if (isPaused != paused)
        {
            isPaused = paused;
            pauseTextObj.SetActive(isPaused);
            UpdateRandomizeButtonState(isPaused);
        }
    }

    public void UpdateRandomizeButtonState(bool enabled)
    {
        buttonComp.interactable = enabled;

        if (enabled)
        {
            buttonImage.color = colors["rButtonActive"];
        }
        else
        {
            buttonImage.color = colors["rButtonInactive"];
        }
    }

    public void SetRandomizeButtonCallback(Action callback)
    {
        buttonComp.onClick.RemoveAllListeners();
        buttonComp.onClick.AddListener(() => callback?.Invoke());
    }

    public void SetSliderCallback(Action callback)
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener((value) => callback?.Invoke());
    }

    public float GetSpeedValue()
    {
        return Mathf.Clamp(5.0f * slider.value, 0.1f, 5.0f);
    }

    public void UpdateStates()
    {
        for (int x = 0; x < gridCell.GetLength(0); x++)
        {
            for (int y = 0; y < gridCell.GetLength(1); y++)
            {
                ChangeCellColor(x, y, colors["border"], colors[gridCell[x, y].state]);
            }
        }
    }

    public void UpdateSelection()
    {
        Vector2 cellUnderCursor = GetCellUnderCursor();
        int gridX = Mathf.FloorToInt(cellUnderCursor.x);
        int gridY = Mathf.FloorToInt(cellUnderCursor.y);
        if (gridX != -1)
        {
            if (gridCell[gridX, gridY].state == "alive")
            {
                ChangeCellColor(gridX, gridY, colors["border"], colors["selectedA"]);
            }
            else
            {
                ChangeCellColor(gridX, gridY, colors["border"], colors["selectedD"]);
            }
        }
    }

    Vector2 GetCellUnderCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int gridX = Mathf.FloorToInt(mousePos.x + 0.5f);
        int gridY = Mathf.FloorToInt(mousePos.y + 0.5f);

        if (gridX >= gridCell.GetLength(0) || gridX < 0 ||
            gridY >= gridCell.GetLength(1) || gridY < 0)
        {
            return new Vector2(-1, -1);
        }

        return new Vector2(gridX, gridY);
    }

    void ChangeCellColor(int gridX, int gridY, Color borderColor, Color intColor)
    {
        GameObject cell = gridObj[gridX, gridY];

        Transform interior = cell.transform.Find("Interior");
        SpriteRenderer intRenderer = interior.GetComponent<SpriteRenderer>();
        intRenderer.color = intColor;

        Transform border = cell.transform.Find("Border");
        SpriteRenderer borderRenderer = border.GetComponent<SpriteRenderer>();
        borderRenderer.color = borderColor;
    }

    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            return color;
        }
        return Color.white;
    }
}