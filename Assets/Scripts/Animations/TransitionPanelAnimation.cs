using UnityEngine;
using UnityEngine.UI;

public class TransitionPanelAnimation : MonoBehaviour
{
    private bool fadeIn;
    private bool restart;
    private float duration;
    private float fraction;

    private ScenesManager.Scene baseScene;
    private ScenesManager.Scene transitionScene;
    private Color baseColor;
    private Color transitionColor;

    public void SetAnimation(bool r, float t, ScenesManager.Scene bs, ScenesManager.Scene ts)
    {
        fadeIn = true;
        restart = r;
        duration = t;
        fraction = 0;

        baseScene = bs;
        transitionScene = ts;

        ScenesManager.ins.transition = true;
    }

    private void Awake()
    {
        baseColor = GetComponent<Image>().color;
        transitionColor = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (fraction >= 1)
            {
                if (restart)
                {
                    GameManager.ins.RestartGame();
                }

                if (baseScene != transitionScene)
                {
                    ScenesManager.ins.HideScene(baseScene);
                    ScenesManager.ins.UnhideScene(transitionScene);
                }

                if (GameManager.ins.paused)
                {
                    ScenesManager.ins.pauseScreen.Play("Idle");
                    GameManager.ins.UnpauseGame();
                }

                fraction = 1.0f;
                fadeIn = false;
            }

            fraction += Time.deltaTime / duration * 2;
        }
        else
        {
            if (fraction <= 0)
            {
                ScenesManager.ins.transition = false;
                gameObject.SetActive(false);
            }

            fraction -= Time.deltaTime / duration * 2;
        }
    
        Color c = GetComponent<Image>().color;
        GetComponent<Image>().color = Color.Lerp(baseColor, transitionColor, fraction);
    }
}
