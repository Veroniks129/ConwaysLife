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

    private int gridWidth;
    private int gridHeight;

    //public Graphics(GameObject[,] gridObj, Cell[,] gridCell)
    //{
    //    this.gridObj = gridObj;
    //    this.gridCell = gridCell;
    //    this.gridWidth = gridCell.GetLength(0);
    //    this.gridHeight = gridCell.GetLength(1);

    //    colors = new Dictionary<string, Color>();
    //    foreach (string key in colorsHex.Keys)
    //    {
    //        colors[key] = HexToColor(colorsHex[key]);
    //    }

    //    CreateBackground();
    //    CreatePauseText();
    //    //CreateRandomizeButton();
    //    CreateSpeedInputField();
    //}

    //public void AddGrids(GameObject[,] gridObj, Cell[,] gridCell)
    //{
    //    this.gridObj = gridObj;
    //    this.gridCell = gridCell;
    //    this.gridWidth = gridCell.GetLength(0);
    //    this.gridHeight = gridCell.GetLength(1);
    //}

    public void Update()
    {
       //SetRandomizeButtonPosition();
       //SetSliderPosition();
    }

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

        speedText = speedTextObj.GetComponent<TextMeshProUGUI>();
        slider = sliderObj.GetComponent<Slider>();
        buttonComp = randomizeButton.GetComponent<Button>();
        buttonImage = randomizeButton.GetComponent<Image>();

        CreateBackground();
        CreatePauseText();

        SetGUIPosition();
        slider.value = 0.2f;
        UpdateSpeedText(1.0f);
        
        //CreateRandomizeButton();
        //CreateSpeedInputField();
    }

    void CreateBackground()
    {
        backgroundObj = new GameObject("Background");
        SpriteRenderer bgRenderer = backgroundObj.AddComponent<SpriteRenderer>();

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, colors["border"]);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
        bgRenderer.sprite = sprite;

        Camera mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        backgroundObj.transform.localScale = new Vector3(cameraWidth, cameraHeight, 1);
        backgroundObj.transform.position = new Vector3(
            mainCamera.transform.position.x,
            mainCamera.transform.position.y,
            10
        );
    }

    void CreatePauseText()
    {
        GameObject canvasObj = GameObject.Find("UICanvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("UICanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        pauseTextObj = new GameObject("PauseText");
        pauseTextObj.transform.SetParent(canvasObj.transform);

        pauseText = pauseTextObj.AddComponent<Text>();
        pauseText.text = "PAUSED";

        pauseText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (pauseText.font == null)
        {
            pauseText.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        }

        pauseText.fontSize = 24;
        pauseText.color = Color.white;
        pauseText.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = pauseTextObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(200, 50);

        pauseTextObj.SetActive(false);
    }

    //void CreateRandomizeButton()
    //{
    //    GameObject canvasObj = GameObject.Find("GameCanvas");
    //    if (canvasObj == null)
    //    {
    //        canvasObj = new GameObject("UICanvas");
    //        Canvas canvas = canvasObj.AddComponent<Canvas>();
    //        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    //        canvasObj.AddComponent<CanvasScaler>();
    //        canvasObj.AddComponent<GraphicRaycaster>();
    //    }

    //    randomizeButton = new GameObject("RandomizeButton");
    //    randomizeButton.transform.SetParent(canvasObj.transform);

    //    buttonComponent = randomizeButton.AddComponent<Button>();
    //    buttonImage = randomizeButton.AddComponent<Image>();
    //    buttonImage.color = colors["buttonInactive"];

    //    GameObject textObj = new GameObject("ButtonText");
    //    textObj.transform.SetParent(randomizeButton.transform);

    //    Text buttonText = textObj.AddComponent<Text>();
    //    buttonText.text = "Randomize";
    //    buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
    //    buttonText.fontSize = 14;
    //    buttonText.color = Color.white;
    //    buttonText.alignment = TextAnchor.MiddleCenter;

    //    RectTransform buttonRect = randomizeButton.GetComponent<RectTransform>();
    //    buttonRect.sizeDelta = new Vector2(120, 40);

    //    RectTransform textRect = textObj.GetComponent<RectTransform>();
    //    textRect.anchorMin = Vector2.zero;
    //    textRect.anchorMax = Vector2.one;
    //    textRect.offsetMin = Vector2.zero;
    //    textRect.offsetMax = Vector2.zero;

    //    buttonComponent.interactable = false;
    //    SetRandomizeButtonPosition();
    //}

    //void CreateSpeedInputField()
    //{
    //    GameObject canvasObj = GameObject.Find("UICanvas");
    //    if (canvasObj == null)
    //    {
    //        canvasObj = new GameObject("UICanvas");
    //        Canvas canvas = canvasObj.AddComponent<Canvas>();
    //        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    //        canvasObj.AddComponent<CanvasScaler>();
    //        canvasObj.AddComponent<GraphicRaycaster>();
    //    }

    //    speedInputFieldObj = new GameObject("SpeedInputField");
    //    speedInputFieldObj.transform.SetParent(canvasObj.transform);

    //    Image inputBackground = speedInputFieldObj.AddComponent<Image>();
    //    inputBackground.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

    //    speedInputField = speedInputFieldObj.AddComponent<InputField>();

    //    GameObject textObj = new GameObject("InputText");
    //    textObj.transform.SetParent(speedInputFieldObj.transform);
    //    speedInputText = textObj.AddComponent<Text>();
    //    speedInputText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
    //    speedInputText.fontSize = 14;
    //    speedInputText.color = Color.white;
    //    speedInputText.alignment = TextAnchor.MiddleLeft;

    //    GameObject placeholderObj = new GameObject("Placeholder");
    //    placeholderObj.transform.SetParent(speedInputFieldObj.transform);
    //    speedPlaceholderText = placeholderObj.AddComponent<Text>();
    //    speedPlaceholderText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
    //    speedPlaceholderText.fontSize = 14;
    //    speedPlaceholderText.color = new Color(0.7f, 0.7f, 0.7f, 0.8f);
    //    speedPlaceholderText.alignment = TextAnchor.MiddleLeft;
    //    speedPlaceholderText.text = "Speed (0.1 - 5.0)";

    //    RectTransform inputRect = speedInputFieldObj.GetComponent<RectTransform>();
    //    inputRect.sizeDelta = new Vector2(60, 35);

    //    RectTransform textRect = textObj.GetComponent<RectTransform>();
    //    textRect.anchorMin = Vector2.zero;
    //    textRect.anchorMax = Vector2.one;
    //    textRect.offsetMin = new Vector2(10, 5);
    //    textRect.offsetMax = new Vector2(-10, -5);

    //    RectTransform placeholderRect = placeholderObj.GetComponent<RectTransform>();
    //    placeholderRect.anchorMin = Vector2.zero;
    //    placeholderRect.anchorMax = Vector2.one;
    //    placeholderRect.offsetMin = new Vector2(10, 5);
    //    placeholderRect.offsetMax = new Vector2(-10, -5);

    //    speedInputField.textComponent = speedInputText;
    //    speedInputField.placeholder = speedPlaceholderText;
    //    speedInputField.contentType = InputField.ContentType.DecimalNumber;
    //    speedInputField.characterLimit = 4;

    //    speedInputField.text = "1.0";

    //    UpdateSpeedInputFieldPosition();
    //}

    //void UpdatePauseTextPosition()
    //{
    //    if (pauseTextObj != null)
    //    {
    //        Vector3 worldPosition = new Vector3(gridWidth / 2f - 0.5f, gridHeight + 1f, 0);
    //        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

    //        RectTransform textRect = pauseTextObj.GetComponent<RectTransform>();
    //        textRect.position = screenPosition;
    //    }
    //}

    void SetGUIPosition()
    {
        const int objCnt = 4;
        Vector2[] positions = new Vector2[objCnt]
        {
            new Vector2(0.1f, 0.8f), //randomize
            new Vector2(0.1f, 0.65f), //slider
            new Vector2(0.1f, 0.6f), // speed text
            new Vector2(0.5f, 1f) //pause text
        };

        Vector2[] biases = new Vector2[objCnt]
        {
            new Vector2(0, 0),
            new Vector2(5, 0),
            new Vector2(0, 0),
            new Vector2(-1f, 0)
        };

        GameObject[] objects = new GameObject[objCnt]
        {
            randomizeButton,
            sliderObj,
            speedTextObj,
            pauseTextObj
        };

        //SetAnchoredPosition(randomizeButton, positions[0], biases[0]);
        //SetAnchoredPosition(sliderObj, positions[1], biases[1]);
        //SetAnchoredPosition(speedText, positions[2], biases[2]);

        for (int i = 0; i < 4; i++)
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

    //void SetRandomizeButtonPosition()
    //{
    //    //Vector3 worldPosition = new Vector3(0.1f * gridWidth, gridHeight + 1f, 0);
    //    //Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

    //    //RectTransform buttonRect = randomizeButton.GetComponent<RectTransform>();
    //    //buttonRect.position = screenPosition;

    //    RectTransform rt = randomizeButton.GetComponent<RectTransform>();
    //    rt.anchorMin = new Vector2(0.1f, 0.8f);
    //    rt.anchorMax = new Vector2(0.1f, 0.8f); 
    //    rt.pivot = new Vector2(0.1f, 0.8f);
    //    rt.anchoredPosition = new Vector2(0, 0);
    //}

    //void SetSliderPosition()
    //{
    //    //Vector3 worldPosition = new Vector3(gridWidth * 0.85f, gridHeight + 1f, 0);
    //    //Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

    //    //RectTransform buttonRect = sliderObj.GetComponent<RectTransform>();
    //    //buttonRect.position = screenPosition;

    //    RectTransform rt = slider.GetComponent<RectTransform>();
    //    rt.anchorMin = new Vector2(0.1f, 0.7f);
    //    rt.anchorMax = new Vector2(0.1f, 0.7f);
    //    rt.pivot = new Vector2(0.1f, 0.7f);
    //    rt.anchoredPosition = new Vector2(5, 0);
    //}

    public void UpdateSpeedText(float value)
    {
        speedText.text = $"Speed: {value:F1} it/s";
    }

    //void UpdateSpeedInputFieldPosition()
    //{
    //    if (speedInputFieldObj != null)
    //    {
    //        Vector3 worldPosition = new Vector3(gridWidth - 3f, gridHeight + 1f, 0);
    //        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

    //        RectTransform inputRect = speedInputFieldObj.GetComponent<RectTransform>();
    //        inputRect.position = screenPosition;
    //    }
    //}

    public void UpdatePauseVisualization(bool paused)
    {
        if (isPaused != paused)
        {
            isPaused = paused;

            if (pauseTextObj != null)
            {
                pauseTextObj.SetActive(isPaused);
                if (isPaused)
                {
                    //UpdatePauseTextPosition();
                }
            }

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

        //slider.onEndEdit.RemoveAllListeners();
        //slider.onEndEdit.AddListener((value) => callback?.Invoke(value));
    }

    public float GetSpeedValue()
    {
        //if (speedInputField != null && !string.IsNullOrEmpty(speedInputField.text))
        //{
        //    string text = speedInputField.text.Replace(",", ".");
        //    if (float.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float speed))
        //    {
        //        return Mathf.Clamp(speed, 0.1f, 5.0f);
        //    }
        //}
        //return 1.0f;
        return Mathf.Clamp(5.0f * slider.value, 0.1f, 5.0f);
    }

    //public void SetSpeedValue(float speed)
    //{
    //    if (speedInputField != null)
    //    {
    //        speedInputField.text = Mathf.Clamp(speed, 0.1f, 5.0f).ToString("F1");
    //    }
    //}

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
            try
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
            catch (Exception e)
            {
                Debug.LogError($"graphics.UpdateSelection: {e.Message}");
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