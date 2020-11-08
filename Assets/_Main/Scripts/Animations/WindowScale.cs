using UnityEngine;

public class WindowScale : MonoBehaviour
{
    public enum Mode { ScaleIn, ScaleOut };

    private Mode mode;
    private float duration;
    private float scale;
    private float time;
    private RectTransform trans;

    public void SetAnimation(float d, Mode m)
    {
        mode = m;
        duration = d;
        scale = mode == Mode.ScaleIn ? 0.0f : 1.0f;
        time = 0.0f;
        trans = GetComponent<RectTransform>();
    }

    private void Update()
	{
        trans.localScale = new Vector3(scale, scale, scale);

        if (mode == Mode.ScaleIn)
        {
            if (scale >= 1.0f)
            {
                trans.localScale = Vector3.one;
                enabled = false;
            }

            time += Time.deltaTime;
            scale = EaseIn(time, 0.0f, 1.0f, duration);
        }
        else if (mode == Mode.ScaleOut)
        {
            if (scale <= 0.0f)
            {
                trans.localScale = Vector3.zero;
                gameObject.SetActive(false);
                enabled = false;
            }

            time += Time.deltaTime;
            scale = EaseIn(time, 1.0f, -1.0f, duration);
        }
	}

    private float EaseIn(float t, float s, float c, float d)
    {
        t /= d;
        return c * t * t + s;
    }
}
