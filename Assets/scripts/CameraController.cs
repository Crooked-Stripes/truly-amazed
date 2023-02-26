using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Tilemap mazeTilemap;
    [SerializeField] private float padding = 1f;
    
    public Camera mainCamera;

    public void ResizeCamera()
    {
        if (mazeTilemap == null)
        {
            return;
        }


        // Get the size of the Tilemap in Unity units
        Vector3 tilemapSize = Vector3.Scale(mazeTilemap.size, mazeTilemap.cellSize);

        // Calculate the aspect ratio of the screen
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the size of the Camera view that fits the entire Tilemap
        float cameraSize = Mathf.Max(tilemapSize.x / aspectRatio, tilemapSize.y) / 2f;

        // Add padding to the Camera size
        cameraSize += padding;

        //changing camera position
        mainCamera.transform.position = new Vector3(tilemapSize.x / 2f - 0.5f, tilemapSize.y / 2f - 0.5f, -10f);

        // Set the size of the Camera view
        mainCamera.orthographicSize = cameraSize;
    }

}
