using UnityEngine;

public class BlockFallAnimation : MonoBehaviour
{
    public enum Direction { Left, Right };
    public enum Phase { Up, Down };

    private float duration;
    private float[] fractions = new float[2];
    private Phase phase;
    private Vector3 basePosition;
    private Vector3 rotation;
    private Vector3[] positions = new Vector3[2];

    public void SetAnimation(float t, Direction d)
    {
        duration = t;
        fractions[0] = fractions[1] = 0;
        phase = Phase.Up;
        basePosition = transform.position;

        rotation = new Vector3(0, 0, d == Direction.Left ? 45 : -45);

        float x = d == Direction.Left ? -0.25f : 0.25f;
        Vector3 p = basePosition;

        positions[0] = new Vector3(p.x + x, p.y + 0.3f, p.z);
        positions[1] = new Vector3(p.x + x * 2.5f, p.y - 20, p.z);
    }

    private void Update()
    {
        if (fractions[0] >= 1)
        {
            if (phase == Phase.Up)
            {
                fractions[0] = 0;
                phase = Phase.Down;
                basePosition = positions[0];
            }
            else
            {
                enabled = false;
            }
        }

        if (phase == Phase.Up)
            fractions[0] += Time.deltaTime / duration / 0.1f;
        else
            fractions[0] += Time.deltaTime / duration / 0.9f;
        fractions[1] += Time.deltaTime / duration / 0.5f;

        transform.position = Vector3.Lerp(basePosition, positions[(int)phase], fractions[0]);
        transform.eulerAngles = Vector3.Lerp(Vector3.zero, rotation, fractions[1]);
    }
}
