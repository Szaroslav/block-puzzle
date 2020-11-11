using UnityEngine;
using System.Collections;

public class ProgressManager : MonoBehaviour
{
    public static void LoadProgress()
    {
        bool[] audioMutes = GetAudioMutes();
        float[] audioVolumes = GetAudioVolumes();

        AudioManager.ins.ChangeMusicVolume(audioVolumes[0]);
        AudioManager.ins.ChangeSoundVolume(audioVolumes[1]);
        AudioManager.ins.ChangeSliderValues(audioVolumes);

        if (audioMutes[0])
            AudioManager.ins.MuteMusic();
        else
            AudioManager.ins.UnmuteMusic();
        if (audioMutes[1])
            AudioManager.ins.MuteSound();
        else
            AudioManager.ins.UnmuteSound();

        GameManager.ins.firstBeatenScore = PlayerPrefs.HasKey("firstBeatenScore") ? GetBool("firstBeatenScore") : true;
        GameManager.ins.continueGame = PlayerPrefs.HasKey("continueGame") ? GetBool("continueGame") : true;
        GameManager.ins.score = PlayerPrefs.GetInt("score");
        GameManager.ins.scoreText.text = GameManager.ins.score.ToString();
        GameManager.ins.bestScore = GetBestScore();
        GameManager.ins.bestScoreText.text = GameManager.ins.bestScore.ToString();

        if (!GameManager.ins.firstBeatenScore)
            GameManager.ins.bestScoreIconLayer.GetComponent<Animator>().Play("Beaten score");

        for (int y = 0; y < BoardManager.BOARD_SIZE; y++)
        {
            for (int x = 0; x < BoardManager.BOARD_SIZE; x++)
            {
                Color color = GetColor(GetBlockKey(x, y));

                if (color != Color.black)
                {
                    BlockTile b = BoardManager.ins.SpawnBlockTile(x, y);
                    b.GetComponent<SpriteRenderer>().color = color;
                    b.defaultColor = color;
                }
            }
        }

        for (int i = 0; i < BoardManager.BLOCKS_AMOUNT; i++)
        {
            if (PlayerPrefs.HasKey(i + "block"))
            {
                int prefabIndex = PlayerPrefs.GetInt(i + "block");
                BoardManager.ins.blocks[i] = BoardManager.ins.SpawnBlock(i, prefabIndex);
            }
        }
    }

    public static void SaveProgress()
    {
        SetAudioMutes(AudioManager.ins.GetAudioMutes());
        SetAudioVolumes(AudioManager.ins.GetAudioVolumes());
        SetBestScore(GameManager.ins.bestScore);

        // // // // // // // // //      SAVE BOARD PROGRESS (IS NOT GAME OVER)      // // // // // // // // //
        if (!GameManager.ins.gameOver)
        {
            SetBool("firstBeatenScore", GameManager.ins.firstBeatenScore);
            SetBool("continueGame", GameManager.ins.continueGame);
            PlayerPrefs.SetInt("score", GameManager.ins.score);

            for (int y = 0; y < BoardManager.BOARD_SIZE; y++)
            {
                for (int x = 0; x < BoardManager.BOARD_SIZE; x++)
                {
                    if (BoardManager.ins.boardBlocks[x, y])
                    {
                        Color color = BoardManager.ins.boardBlocks[x, y].defaultColor;
                        SetColor(GetBlockKey(x, y), color);
                    }
                    else
                    {
                        SetColor(GetBlockKey(x, y), Color.black);
                    }
                }
            }

            for (int i = 0; i < BoardManager.BLOCKS_AMOUNT; i++)
                PlayerPrefs.SetInt(i + "block", BoardManager.ins.blocks[i].prefabIndex);
        }
        // // // // // // // // //      DO NOT SAVE BOARD PROGRESS (IS GAME OVER)      // // // // // // // // //
        else
        {
            SetBool("firstBeatenScore", true);
            SetBool("continueGame", true);
            PlayerPrefs.SetInt("score", 0);

            for (int y = 0; y < BoardManager.BOARD_SIZE; y++)
                for (int x = 0; x < BoardManager.BOARD_SIZE; x++)
                        SetColor(GetBlockKey(x, y), Color.black);

            for (int i = 0; i < BoardManager.BLOCKS_AMOUNT; i++)
                PlayerPrefs.SetInt(i + "block", BoardManager.Rand(0, BoardManager.BLOCK_PREFABS_AMOUNT));
        }
    }

    public static bool[] GetAudioMutes()
    {
        bool[] am = new bool[2];

        am[0] = PlayerPrefs.HasKey("musicMute") ? GetBool("musicMute") : false;
        am[1] = PlayerPrefs.HasKey("soundMute") ? GetBool("soundMute") : false;

        return am;
    }

    public static float[] GetAudioVolumes()
    {
        float[] av = new float[2];

        av[0] = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 0.4f;
        av[1] = PlayerPrefs.HasKey("soundVolume") ? PlayerPrefs.GetFloat("soundVolume") : 1.0f;

        return av;
    }

    public static void SetAudioMutes(bool[] v)
    {
        SetBool("musicMute", v[0]);
        SetBool("soundMute", v[1]);
    }

    public static void SetAudioVolumes(float[] v)
    {
        PlayerPrefs.SetFloat("musicVolume", v[0]);
        PlayerPrefs.SetFloat("soundVolume", v[1]);
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt("bestScore");
    }

    public static void SetBestScore(int s = 0)
    {
        int bs = GameManager.ins.bestScore;

        if (s > bs)
        {
            GameManager.ins.bestScoreText.GetComponent<ScoreAddAnimation>().enabled = true;
            GameManager.ins.bestScoreText.GetComponent<ScoreAddAnimation>().SetAnimation(s - bs, bs, 0.4f);
            GameManager.ins.bestScore = s;
            PlayerPrefs.SetInt("bestScore", s);

            if (GameManager.ins.firstBeatenScore)
            {
                GameManager.ins.bestScoreIconLayer.GetComponent<Animator>().Play("Beaten score");
                GameManager.ins.firstBeatenScore = false;
            }
        }
    }

    private void Awake()
	{
        StartCoroutine(Wait());
	}

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.02f);

        LoadProgress();
        BoardManager.ins.CheckBoard(true);
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            SaveProgress();
    }

    private static bool GetBool(string k)
    {
        if (PlayerPrefs.GetInt(k) == 0)
            return false;

        return true;
    }

    private static void SetBool(string k, bool v)
    {
        if (!v)
            PlayerPrefs.SetInt(k, 0);
        else
            PlayerPrefs.SetInt(k, 1);
    }

    private static string GetBlockKey(int x, int y)
    {
        return "block[" + x + ", " + y;
    }

    private static Color GetColor(string k)
    {
        float[] c = GetFloatArray(k, 4);

        if (c == null)
            return Color.black;

        return new Color(c[0], c[1], c[2], c[3]);
    }

    private static void SetColor(string k, Color c)
    {
        float[] comp = new float[] { c.r, c.g, c.b, c.a };
        SetFloatArray(k, comp);
    }

    private static float[] GetFloatArray(string k, int s)
    {
        float[] arr = new float[s];
        
        if (!PlayerPrefs.HasKey(k))
        {
            Debug.LogError("The float array does not exist!");
            return null;
        }

        for (int i = 0; i < s; i++)
            arr[i] = PlayerPrefs.GetFloat(i + k);

        return arr;
    }

    private static void SetFloatArray(string k, float[] arr)
    {
        PlayerPrefs.SetFloat(k, 0);

        for (int i = 0; i < arr.Length; i++)
            PlayerPrefs.SetFloat(i + k, arr[i]);
    }
}
