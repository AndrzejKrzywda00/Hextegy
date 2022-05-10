using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Vector3 _camPosition;
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private float reactionSpaceThickness = 10f;
    [SerializeField] private Vector2 movementLimit = new Vector2(5, 5);

    [SerializeField] private Camera cam;
    [SerializeField] private float scrollSpeed = 20f;
    private const float NormalizationValueForScrollSpeed = 100f;
    private float _orthographicSize;
    [SerializeField] private float maxOrthographicSize = 10f;
    [SerializeField] private float minOrthographicSize = 1f;
    
    private Vector3 _dragOrigin;

    public void Start() {
        cam = Camera.main;
    }

    public void Update() {
        MoveWhenKeyPressedOrMouseNearScreenEdge();
        Scroll();
        DragIfMouseScrollPressed();
    }

    private void MoveWhenKeyPressedOrMouseNearScreenEdge() {
        _camPosition = transform.position;
        
        //order matters
        MakeMove();
        RestrictMovement();
        
        transform.position = _camPosition;
    }

    private void MakeMove() {
        if (IsMovingUp()) _camPosition.y += movementSpeed * Time.deltaTime;
        if (IsMovingDown()) _camPosition.y -= movementSpeed * Time.deltaTime;
        if (IsMovingRight()) _camPosition.x += movementSpeed * Time.deltaTime;
        if (IsMovingLeft()) _camPosition.x -= movementSpeed * Time.deltaTime;
    }

    private bool IsMovingUp() {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || IsNearUpperScreenEdge();
    }

    private bool IsNearUpperScreenEdge() {
        return Input.mousePosition.y >= Screen.height - reactionSpaceThickness;
    }

    private bool IsMovingDown() {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || IsNearBottomScreenEdge();
    }

    private bool IsNearBottomScreenEdge() {
        return Input.mousePosition.y <= reactionSpaceThickness;
    }

    private bool IsMovingRight() {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || IsNearRightScreenEdge();
    }

    private bool IsNearRightScreenEdge() {
        return Input.mousePosition.x >= Screen.width - reactionSpaceThickness;
    }

    private bool IsMovingLeft() {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || IsNearLeftScreenEdge();
    }

    private bool IsNearLeftScreenEdge() {
        return Input.mousePosition.x <= reactionSpaceThickness;
    }

    private void RestrictMovement() {
        _camPosition.x = Mathf.Clamp(_camPosition.x, -movementLimit.x, movementLimit.x);
        _camPosition.y = Mathf.Clamp(_camPosition.y, -movementLimit.y, movementLimit.y);
    }

    private void Scroll() {
        _orthographicSize = cam.orthographicSize;

        AdjustOrthographicSize();
        RestrictScrolling();
        
        cam.orthographicSize = _orthographicSize;
    }

    private void AdjustOrthographicSize() {
        var scrollMagnitude = Input.GetAxis("Mouse ScrollWheel");
        _orthographicSize -= scrollMagnitude * Time.deltaTime * scrollSpeed * NormalizationValueForScrollSpeed;
    }

    private void RestrictScrolling() {
        _orthographicSize = Mathf.Clamp(_orthographicSize, minOrthographicSize, maxOrthographicSize);
    }

    private void DragIfMouseScrollPressed() {
        Drag();
        RestrictMovement();

        transform.position = _camPosition;
    }

    private void Drag() {
        if (Input.GetKeyDown(KeyCode.Mouse2)) {
            SaveClickPosition();
        }
        if (Input.GetKey(KeyCode.Mouse2)) {
            MoveCameraWithCursor();
        }
    }

    private void SaveClickPosition() {
        _dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void MoveCameraWithCursor() {
        _camPosition += _dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
