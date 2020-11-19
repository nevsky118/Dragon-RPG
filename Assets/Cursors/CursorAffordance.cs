using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D questionCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    CameraRaycaster cameraRaycaster;

    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnDelegateCalled; // registering 
    }

    void OnDelegateCalled(Layer newLayer) // Only called when layer changes
    {
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Don't know what cursor tto show");
                return;
        }
    }
}
