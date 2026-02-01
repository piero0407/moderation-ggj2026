using UnityEngine;

// Attach this script to a GameObject with a Collider, then mouse over the object to see your cursor change.
public class CursorSwap : MonoBehaviour
{
    [SerializeField] public Texture2D cursorTexture;

    void Awake()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(65, 60), CursorMode.Auto);
    }
}