using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 100f;

    public bool gameStarted = false;

    public void Setup(float width, float height, float scale)
    {
        Vector3 position = new Vector3(width / 2, height / 2, -10f);
        Camera.main.transform.position = position;
        Camera.main.orthographicSize = (float)(scale / 2 * 1.1);
        

        DragCamera2D dragCamera = FindObjectOfType<DragCamera2D>();
        dragCamera.zoomEnabled = true;
        dragCamera.dragEnabled = true;
        dragCamera.minZoom = 2f;
        dragCamera.maxZoom = scale * 1.1f / 2f;
    }

    public void UnenableZoom()
    {
        DragCamera2D dragCamera = FindObjectOfType<DragCamera2D>();
        dragCamera.zoomEnabled = false;
        dragCamera.dragEnabled = false;
    }
}