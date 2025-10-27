using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    public Button startButton;
    Game game;
    public GameObject startScreen;

    void Start()
    {
        game = FindObjectOfType<Game>();
        game.GameStartRequested += TransitionToGame;
        startButton.onClick.AddListener(ButtonClick);
    }

    public void LoadScreen(float scaleX, float scaleY)
    {
        CameraSetup cameraSetup = FindObjectOfType<CameraSetup>();
        cameraSetup.Setup(0, 0, 6f);
        cameraSetup.UnenableZoom();

        TextMeshProUGUI gameNameText = GameObject.Find("GameName").GetComponent<TextMeshProUGUI>();

        startScreen.transform.position = new Vector3(0, 0, 0);
        RectTransform textRect = gameNameText.GetComponent<RectTransform>();
        textRect.localPosition = new Vector3(-0.44f, 1.46f, 0);
    }

    void SetupButtonPosition()
    {
        RectTransform rectTransform = startButton.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(0f, -1.2f);

        rectTransform.sizeDelta = new Vector2(0.1f, 0.05f);
    }

    void ButtonClick()
    {
        startButton.interactable = false;
        game.RequestGameStart();
    }

    void TransitionToGame()
    {
        StartCoroutine(HideStartScreen());
    }

    IEnumerator HideStartScreen()
    {
        yield return new WaitForSeconds(0f);
        startScreen.SetActive(false);
    }

    void OnDestroy()
    {
        game.GameStartRequested -= TransitionToGame;
    }
}
