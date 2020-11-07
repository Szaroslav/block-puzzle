using UnityEngine;
using UnityEngine.UI;

public class GameOverAnimation : MonoBehaviour
{
    private bool fadeOut;
    private float duration;
    private float fraction;
    private Color nc = new Color(0.416f, 0.992f, 0.557f);
    private Color dc = new Color(0.282f, 0.333f, 0.376f);

    public void SetAnimation(float t, bool f)
    {
        fadeOut = f;
        duration = t;
        fraction = f ? 0 : 1;

        if (!f)
        {
            ScenesManager.ins.UnhideScene(ScenesManager.Scene.Game);
        }
        else
        {
            if (!MonetizationManager.CheckNetworkConnection())
            {
                if (GameManager.ins.continueGame)
                    GameManager.ins.offlineLabel.gameObject.SetActive(true);

                GameManager.ins.continueButton.interactable = false;

                ColorBlock c = GameManager.ins.continueButton.colors;
                c.normalColor = c.disabledColor = dc;
                GameManager.ins.continueButton.colors = c;
            }
            else
            {
                GameManager.ins.offlineLabel.gameObject.SetActive(false);
                GameManager.ins.continueButton.interactable = true;

                ColorBlock c = GameManager.ins.continueButton.colors;
                c.normalColor = c.disabledColor = nc;
                GameManager.ins.continueButton.colors = c;
            }
        }
    }

    private void Update()
    {
        if (fadeOut)
        {
            if (fraction >= 1)
            {
                ScenesManager.ins.HideScene(ScenesManager.Scene.Game);
                ScenesManager.ins.transition = false;
                ScenesManager.ins.gameOverCanvasGroup.GetComponent<Animator>().Play("Start animation");

                GetComponent<CanvasGroup>().interactable = true;
                GetComponent<CanvasGroup>().blocksRaycasts = true;

                enabled = false;
            }

            fraction += Time.deltaTime / duration;

            GetComponent<CanvasGroup>().alpha = fraction;
        }
        else
        {
            if (fraction <= 0)
            {
                ScenesManager.ins.transition = false;

                GameManager.ins.DestroyBlocks();

                GetComponent<CanvasGroup>().interactable = false;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                enabled = false;
            }

            fraction -= Time.deltaTime / duration;

            GetComponent<CanvasGroup>().alpha = fraction;
        }
    }
}
