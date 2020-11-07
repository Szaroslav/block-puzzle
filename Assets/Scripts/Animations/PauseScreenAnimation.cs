using UnityEngine;
using UnityEngine.UI;

public class PauseScreenAnimation : MonoBehaviour
{
    public float backgroundAlpha;
    public int screenY;

    private float duration;
    private float fraction;
    private RectTransform bTrans;
    private Color color;

    public void SetAnimation(float d)
    {
        duration = d;
        fraction = 0.0f;

        bTrans = transform.GetChild(0).GetComponent<RectTransform>();
        color = new Color(0, 0, 0, backgroundAlpha);
    }

    private void Update()
    {
        fraction += Time.deltaTime / duration;

        GetComponent<Image>().color = Color.Lerp(Color.clear, color, fraction);
        bTrans.anchoredPosition = Vector2.Lerp(new Vector2(0, 1300), new Vector2(0, screenY), fraction);

        if (fraction >= 1.0f)
        {
            enabled = false;
        }
    }
}
