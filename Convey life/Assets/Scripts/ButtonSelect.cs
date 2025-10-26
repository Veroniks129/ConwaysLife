using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelect : MonoBehaviour
{
    public string normalColorHex = "FFFFFF";
    public string hoverColorHex = "1976D2";
    public string pressedColorHex = "0D47A1";

    public string buttonTextColorHex = "FFFFFF";

    private Image buttonImage;
    private Color normalColor;
    private Color hoverColor;
    private Color pressedColor;
    private Color buttonTextColor;

    public TextMeshProUGUI buttonText;

    private Button button;

    void Start()
    {
        normalColor = HexToColor(normalColorHex);
        hoverColor = HexToColor(hoverColorHex);
        pressedColor = HexToColor(pressedColorHex);
        buttonTextColor = HexToColor(buttonTextColorHex);

        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

        if (GetComponent<Mask>() == null)
        {
            gameObject.AddComponent<Mask>();
        }
        GetComponent<Mask>().showMaskGraphic = true;

        buttonImage.color = normalColor;
        buttonText.color = buttonTextColor;
    }
    public void OnPointerEnter()
    {
        if (button.interactable)
        {
            buttonImage.color = hoverColor;

        }
    }

    public void OnPointerExit()
    {
        if (button.interactable)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnPointerDown()
    {
        if (button.interactable)
        {
            buttonImage.color = pressedColor;
        }
    }

    public void OnPointerUp()
    {
        if (button.interactable)
        {
            buttonImage.color = hoverColor;
        }
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