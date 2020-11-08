using UnityEngine;

public class BlockScaleAnimation : MonoBehaviour
{
    private Block block;

    private float duration;
    private float fraction;
    private Vector3[] baseScale = new Vector3[2];
    private Vector3[] destinationScale = new Vector3[2];

    public void SetAnimation(bool s, float t)
    {
        duration = t;
        fraction = 0;

        baseScale[0] = transform.localScale;
        baseScale[1] = s ? block.scaledScale : block.baseScale;
        destinationScale[0] = s ? Vector3.one : new Vector3(0.6f, 0.6f, 0.6f);
        destinationScale[1] = s ? block.baseScale : block.scaledScale;
    }

    private void Awake()
    {
        block = GetComponent<Block>();
    }

    private void Update()
    {
        if (fraction >= 1)
        {
            enabled = false;
        }

        fraction += Time.deltaTime / duration;

        transform.localScale = Vector3.Lerp(baseScale[0], destinationScale[0], fraction);
        block.ScaleTiles(Vector3.Lerp(baseScale[1], destinationScale[1], fraction));
    }
}
