using UnityEngine;

public class BlockFadeAnimation : MonoBehaviour
{
    private float duration;
    private float fraction;
    private Color currentColor;
    private Color color;

    public void SetAnimation(float d, Color c)
    {
        if (d != 0.0f)
        {
            duration = d;
            fraction = 0.0f;
            currentColor = GetComponent<SpriteRenderer>().color;
            color = c;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = c;
            enabled = false;
        }
    }

    private void Update()
    {
        if (fraction >= 1)
            enabled = false;

        fraction += Time.deltaTime / duration;

        GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, color, fraction);
    }
}
