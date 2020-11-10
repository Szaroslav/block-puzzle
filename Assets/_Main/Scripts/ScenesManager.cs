using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager ins;

    public enum Scene { Menu, Settings, Game, GameOverScreen };

    public TransitionPanelAnimation transitionPanel;

    public CanvasGroup menuCanvasGroup;
    public CanvasGroup settingsCanvasGroup;
    public Transform gameTransform;
    public CanvasGroup gameCanvasGroup;
    public CanvasGroup gameOverCanvasGroup;

    [HideInInspector]
    public bool transition = false;
    
    private Scene currentScene = Scene.Menu;
    private PauseScreen pauseScreen;

    public void LoadMenu()
    {
        if (!transition && Input.touches.Length < 2)
        {
            transitionPanel.gameObject.SetActive(true);
            transitionPanel.SetAnimation(false, Constants.SCENE_TRANSITION_DUR, currentScene, Scene.Menu);

            currentScene = Scene.Menu;

            GameManager.ins.ChangeBlocksColor();
        }
    }

    public void LoadSettings()
    {
        if (!transition)
        {
            transitionPanel.gameObject.SetActive(true);
            transitionPanel.SetAnimation(false, Constants.SCENE_TRANSITION_DUR, currentScene, Scene.Settings);

            currentScene = Scene.Settings;
        }
    }

    public void LoadGame()
    {
        if (!transition)
        {
            transitionPanel.gameObject.SetActive(true);
            transitionPanel.SetAnimation(false, Constants.SCENE_TRANSITION_DUR, currentScene, Scene.Game);

            currentScene = Scene.Game;
        }
    }

    public void RestartGame()
    {
        if (!transition && Input.touches.Length < 2)
        {
            transitionPanel.gameObject.SetActive(true);
            transitionPanel.SetAnimation(true, Constants.SCENE_TRANSITION_DUR, currentScene, Scene.Game);

            currentScene = Scene.Game;
        }
    }

    public void LoadGameOverScreen()
    {
        if (GameManager.ins.continueGame)
            GameManager.ins.continueButton.gameObject.SetActive(true);
        else
            GameManager.ins.continueButton.gameObject.SetActive(false);

        GameOverAnimation anim = gameOverCanvasGroup.GetComponent<GameOverAnimation>();
        anim.enabled = true;
        anim.SetAnimation(0.3f, true);

        gameOverCanvasGroup.GetComponent<Animator>().Play("Idle");
        GameManager.ins.SetGameOver();

        currentScene = Scene.GameOverScreen;
    }

    public void LoadGameFromGameOverScreen()
    {
        GameOverAnimation anim = gameOverCanvasGroup.GetComponent<GameOverAnimation>();
        anim.enabled = true;
        anim.SetAnimation(0.3f, false);

        currentScene = Scene.Game;
    }

    public void HideScene(Scene s)
    {
        if (s == Scene.Menu)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }
        else if (s == Scene.Settings)
        {
            settingsCanvasGroup.alpha = 0;
            settingsCanvasGroup.interactable = false;
            settingsCanvasGroup.blocksRaycasts = false;
        }
        else if (s == Scene.Game)
        {
            gameTransform.gameObject.SetActive(false);
            gameCanvasGroup.alpha = 0;
            gameCanvasGroup.interactable = false;
            gameCanvasGroup.blocksRaycasts = false;
        }
        else if (s == Scene.GameOverScreen)
        {
            gameOverCanvasGroup.alpha = 0;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }
    }

    public void UnhideScene(Scene s)
    {
        if (s == Scene.Menu)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        }
        else if (s == Scene.Settings)
        {
            settingsCanvasGroup.alpha = 1;
            settingsCanvasGroup.interactable = true;
            settingsCanvasGroup.blocksRaycasts = true;
        }
        else if (s == Scene.Game)
        {
            gameTransform.gameObject.SetActive(true);
            gameCanvasGroup.alpha = 1;
            gameCanvasGroup.interactable = true;
            gameCanvasGroup.blocksRaycasts = true;
        }
        else if (s == Scene.GameOverScreen)
        {
            gameOverCanvasGroup.alpha = 1;
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        }
    }

    public void ShowPauseScreen()
    {
        if (!GameManager.ins.gameOver)
        {
            pauseScreen.In();
            InputManager.ins.ResetBlock();
        }
    }

    public void HidePauseScreen(bool h = true)
    {
        if (h)
        {
            pauseScreen.Out();
            GameManager.ins.UnpauseGame();
        }
        else
        {
            pauseScreen.Out(true);
            Time.timeScale = 1.0f;
        }    
    }

    private void Awake()
    {
        if (!ins)
            ins = this;

        pauseScreen = FindObjectOfType<PauseScreen>();
    }
}
