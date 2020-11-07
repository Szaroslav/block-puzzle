using UnityEngine;

public class BlockMovingAnimation : MonoBehaviour
{
    private float duration;
    private float fraction;
    private Vector3 startPos;
    private Vector3 destination;

    public void SetAnimation(float t, Vector3 d)
    {
        duration = t;
        fraction = 0;
        startPos = transform.position;
        destination = d;
    }

	private void Update()
	{
        if (fraction >= 1)
        {
            transform.position = destination;
            enabled = false;
        }

        fraction += Time.deltaTime / duration;

        transform.position = Vector3.Lerp(startPos, destination, fraction);
    }
}
