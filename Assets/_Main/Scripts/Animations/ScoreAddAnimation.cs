using UnityEngine;
using TMPro;

public class ScoreAddAnimation : MonoBehaviour
{
    private int points;
    private int score;
    private float duration;
    private float addedPoints;

    public void SetAnimation(int p, int s, float d)
    {
        points = p;
        score = s;
        duration = d;
        addedPoints = 0.0f;
    }

	private void Update()
	{
        addedPoints += points * Time.deltaTime / duration;
        GetComponent<TextMeshProUGUI>().text = (score + (int)addedPoints).ToString();

        if (addedPoints >= points)
        {
            GetComponent<TextMeshProUGUI>().text = (score + points).ToString();
            enabled = false;
        }
	}
}
