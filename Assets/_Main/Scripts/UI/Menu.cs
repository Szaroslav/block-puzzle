using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image logo;
    public Button playButton;
    public Button settingsButton;

    void Start()
    {
        Color c = logo.color; c.a = 0;
        logo.color = c;

        RectTransform rt = playButton.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-800, rt.anchoredPosition.y);

        rt = settingsButton.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(800, rt.anchoredPosition.y);
    }

    public void In()
    {
        LeanTween.alpha(logo.GetComponent<RectTransform>(), 1, 0.5f)
            .setEaseInOutCubic();
        LeanTween.moveX(playButton.GetComponent<RectTransform>(), 0, 0.5f)
            .setEaseInOutCubic()
            .setDelay(0.33f);
        LeanTween.moveX(settingsButton.GetComponent<RectTransform>(), 0, 0.5f)
            .setEaseInOutCubic()
            .setDelay(0.33f);
    }
}
