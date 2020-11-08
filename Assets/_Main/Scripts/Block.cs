using UnityEngine;

public class Block : MonoBehaviour
{
    public int prefabIndex;
    public Color defaultColor;
    public Vector2 size;
    public Vector2Int[] structure;

    [HideInInspector]
    public bool movable = true;
    [HideInInspector]
    public int posIndex;
    [HideInInspector]
    public Vector3 basePosition;
    [HideInInspector]
    public Vector3 baseScale;
    [HideInInspector]
    public Vector3 scaledScale;

    public void SetBasePosition(int i, bool cp = true)
    {
        Vector2 scale = transform.localScale;
        Vector2 colliderSize = GetComponent<BoxCollider>().size * scale;

        Vector3 position = new Vector3(colliderSize.x / 2 - 0.5f + colliderSize.x * i, GameScaler.GetBlockY(), 0);

        basePosition = position;

        if (cp)
            transform.position = basePosition;

        posIndex = i;
    }

    public void Move(float t, Vector3 d)
    {
        GetComponent<BlockMovingAnimation>().enabled = true;
        GetComponent<BlockMovingAnimation>().SetAnimation(t, d);
    }

    public bool IsMoving()
    {
        return GetComponent<BlockMovingAnimation>().enabled;
    }

    public void Scale(bool s, float t)
    {
        GetComponent<BlockScaleAnimation>().enabled = true;
        GetComponent<BlockScaleAnimation>().SetAnimation(s, t);
    }

    public bool IsScaling()
    {
        return GetComponent<BlockScaleAnimation>().enabled;
    }

    public Vector2Int GetFirstCoords()
    {
        Vector3 p;
        p = transform.GetChild(0).transform.position;
        return new Vector2Int((int)(p.x + 0.5f), (int)(p.y + 0.5f));
    }

    public Color GetColor()
    {
        if (transform.GetChild(0).name == "Block tile")
            return transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        return transform.GetChild(1).GetComponent<SpriteRenderer>().color;
    }

    public void ChangeColor(Color c)
    {
        foreach (Transform t in transform)
            if (t.name == "Block tile")
                t.GetComponent<SpriteRenderer>().color = c;
    }

    public void ScaleTiles(Vector3 s)
    {
        foreach (Transform t in transform)
            t.localScale = s;
    }

    private void Awake()
    {
        baseScale = BoardManager.ins.boardTileScale;
        scaledScale = BoardManager.ins.scaledBlockTileScale;

        ScaleTiles(scaledScale);
    }
}
