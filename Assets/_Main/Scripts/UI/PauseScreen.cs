using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    CanvasGroup cg;
    RectTransform rt;
    RectTransform mainRt;
    Image bg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();
        bg = GetComponent<Image>();

        mainRt = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void In()
    {
        CancelAllTweens();
        SetCanvasGroup(true);
        GameManager.ins.PauseGame();

        mainRt.anchoredPosition = new Vector2(0, 1060);

        Color c = bg.color;
        c.a = 0;
        bg.color = c;

        LeanTween.alpha(rt, 0.45f, 0.33f)
            .setEaseInOutCubic()
            .setIgnoreTimeScale(true)
            .setRecursive(false);
        LeanTween.moveY(mainRt, 0, 0.45f)
            .setIgnoreTimeScale(true)
            .setEaseOutBack();
    }

    public void Out(bool instant = false)
    {
        CancelAllTweens();
        
        LeanTween.alpha(rt, 0, instant ? 0.01f : 0.33f)
            .setEaseInOutCubic()
            .setDelay(instant ? 0.15f : 0)
            .setIgnoreTimeScale(true)
            .setRecursive(false);
        LeanTween.moveY(mainRt, 1060, instant ? 0.01f : 0.4f)
            .setEaseInOutCubic()
            .setDelay(instant ? Constants.SCENE_TRANSITION_DUR / 2 : 0)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => {
                SetCanvasGroup(false);
                GameManager.ins.PauseGame();
            });
    }

    void SetCanvasGroup(bool v)
    {
        cg.alpha = v ? 1 : 0;
        cg.interactable = cg.blocksRaycasts = v;
    }

    void CancelAllTweens()
    {
        LeanTween.cancel(gameObject);
        LeanTween.cancel(transform.GetChild(0).gameObject);
    }
}
