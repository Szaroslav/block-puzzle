using UnityEngine;

public class BlockDestroyAnimation : MonoBehaviour
{
    public Sprite destroyedBlock;
    public Vector3 rotation = new Vector3(0, 0, 360);
    public Vector3 scale = new Vector3(0, 0, 0);
    public Color color = new Color(1, 1, 1, 1);

    private float duration;
    private float fraction;

    public void SetAnimation(float t)
    {
        duration = t;
        fraction = 0;
    }

    private void Update()
    {
        if (fraction >= 1)
        {
            if (transform.parent.childCount > 0)
                Destroy(gameObject);
            else
                Destroy(transform.parent.gameObject);
        }
        
        fraction += Time.deltaTime / duration;

        transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), rotation, fraction);
        transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), scale, fraction);
    }
}
