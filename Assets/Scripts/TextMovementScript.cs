using UnityEngine;

public class TextMovementScript : MonoBehaviour
{
    float zoomSpeed = 0.2f, maxScale = 1.1f, minScale = 0.9f, currentScale = 0.9f;
    bool zoomIn = true;

    void Update()
    {
        TxtMovement();
    }

    void TxtMovement()
    {
        if (zoomIn)
            currentScale += zoomSpeed * Time.deltaTime;
        else
            currentScale -= zoomSpeed * Time.deltaTime;

        if (currentScale > maxScale)
            zoomIn = false;
        else if (currentScale < minScale)
            zoomIn = true;

        gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
