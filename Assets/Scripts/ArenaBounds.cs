using UnityEngine;

[ExecuteAlways]
public class ArenaBounds : MonoBehaviour
{
    [Header("Arena Settings")]
    [Tooltip("The total width and height of the rectangular play area.")]
    public Vector2 arenaSize = new Vector2(20f, 10f);
    [Tooltip("How thick the walls are.")]
    public float wallThickness = 0.5f;
    
    [Header("Wall Asset")]
    [Tooltip("Prefab or 1x1 GameObject with a SpriteRenderer and BoxCollider2D to use for walls.")]
    public GameObject wallPrefab;

    private Transform topWall, bottomWall, leftWall, rightWall;

    void Update()
    {
        // Continuously keep the walls snapped to the right size when in play mode or if they exist in edit mode
        if (Application.isPlaying || HasAllWalls())
        {
            UpdateBounds(false); 
        }
    }

    private void OnValidate()
    {
        // Safe resizing while scrubbing values in the editor
        if (HasAllWalls())
        {
            UpdateBounds(false);
        }
    }

    private bool HasAllWalls()
    {
        return transform.Find("TopWall") && transform.Find("BottomWall") && 
               transform.Find("LeftWall") && transform.Find("RightWall");
    }

    public void UpdateBounds(bool createIfMissing = true)
    {
        if (createIfMissing)
        {
            EnsureWallExists(ref topWall, "TopWall");
            EnsureWallExists(ref bottomWall, "BottomWall");
            EnsureWallExists(ref leftWall, "LeftWall");
            EnsureWallExists(ref rightWall, "RightWall");
        }
        else
        {
            topWall = transform.Find("TopWall");
            bottomWall = transform.Find("BottomWall");
            leftWall = transform.Find("LeftWall");
            rightWall = transform.Find("RightWall");
            
            if (topWall == null || bottomWall == null || leftWall == null || rightWall == null) return;
        }

        float halfWidth = arenaSize.x / 2f;
        float halfHeight = arenaSize.y / 2f;
        float offsetX = halfWidth + (wallThickness / 2f);
        float offsetY = halfHeight + (wallThickness / 2f);

        PositionWall(topWall, new Vector2(0, offsetY), new Vector2(arenaSize.x + wallThickness * 2, wallThickness));
        PositionWall(bottomWall, new Vector2(0, -offsetY), new Vector2(arenaSize.x + wallThickness * 2, wallThickness));
        PositionWall(leftWall, new Vector2(-offsetX, 0), new Vector2(wallThickness, arenaSize.y));
        PositionWall(rightWall, new Vector2(offsetX, 0), new Vector2(wallThickness, arenaSize.y));
    }

    public void ClearWalls()
    {
        // Safely destroy in editor or playmode
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.EndsWith("Wall"))
            {
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
        }
        topWall = bottomWall = leftWall = rightWall = null;
    }

    private void EnsureWallExists(ref Transform wall, string wallName)
    {
        wall = transform.Find(wallName);
        if (wall != null) return;

        GameObject newWall;
        if (wallPrefab != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && UnityEditor.PrefabUtility.IsPartOfPrefabAsset(wallPrefab))
            {
                newWall = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(wallPrefab);
                newWall.transform.parent = this.transform;
            }
            else
#endif
            {
                newWall = Instantiate(wallPrefab, this.transform);
            }
            newWall.name = wallName;
        }
        else
        {
            newWall = new GameObject(wallName);
            newWall.transform.parent = this.transform;
            newWall.AddComponent<SpriteRenderer>();
            newWall.AddComponent<BoxCollider2D>();
        }

        wall = newWall.transform;
    }

    private void PositionWall(Transform wall, Vector2 localPos, Vector2 scale)
    {
        wall.localPosition = localPos;
        wall.localScale = scale;
    }
}
