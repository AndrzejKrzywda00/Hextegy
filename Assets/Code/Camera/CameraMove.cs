using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera camera;
    public double radius = 200;
    public double speed = 0.01;
    private Vector3 _dragOrigin;

    void Update()
    {
        MouseMoveToEdge();
        MouseScrollDrag();
        ScrollCamera();
    }
    
    void MouseMoveToEdge()
    {
        Vector3 center = new Vector3(Screen.width, Screen.height, 0) / 2;
        Vector3 move = -1 * (center - Input.mousePosition);
        // TODO -- rebuild this method to work on edges and simplify
        if (move.magnitude > radius) camera.transform.Translate(move.normalized * (float) speed * move.magnitude / 200);
    }

    void MouseScrollDrag()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)) SaveClickPosition();
        if (Input.GetKey(KeyCode.Mouse2)) MoveCameraWithCursor();
    }

    private void MoveCameraWithCursor()
    {
        camera.transform.position += _dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void SaveClickPosition()
    {
        _dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void ScrollCamera()
    {
        if (ZoomingIn() && !CameraHasClosestPosition()) camera.orthographicSize -= (float)0.5;
        if (ZoomingOut() && !CameraHasFarthestPosition()) camera.orthographicSize += (float) 0.5;
    }

    bool ZoomingIn()
    {
        return Input.GetAxis("Mouse ScrollWheel") > 0f;
    }

    bool ZoomingOut()
    {
        return Input.GetAxis("Mouse ScrollWheel") < 0f;
    }

    bool CameraHasFarthestPosition()
    {
        return camera.orthographicSize >= 20;
    }

    bool CameraHasClosestPosition()
    {
        return camera.orthographicSize == 1f;
    }
}
