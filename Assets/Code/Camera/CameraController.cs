using UnityEngine;

public class CameraController : MonoBehaviour {
    
    private Vector3 _camPosition;
    private const float MovementSpeed = 30f;
    private const float ReactionSpaceThickness = 10f;
    private readonly Vector2 _movementLimit = new Vector2(1000, 1000);

    private Camera _cam;
    private const float ScrollSpeed = 50f;
    private const float NormalizationValueForScrollSpeed = 100f;
    private float _orthographicSize;
    private const float MaxOrthographicSize = 200f;
    private const float MinOrthographicSize = 40f;
    
    private Vector3 _dragOrigin;

    public void Start() {
        _cam = Camera.main;
        _cam.transform.SetPositionAndRotation(
            new Vector3(0, 10, 0),
            Quaternion.Euler(90, 0, 0)
            );
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
        if (IsMovingUp()) _camPosition.z += MovementSpeed * Time.deltaTime;
        if (IsMovingDown()) _camPosition.z -= MovementSpeed * Time.deltaTime;
        if (IsMovingRight()) _camPosition.x += MovementSpeed * Time.deltaTime;
        if (IsMovingLeft()) _camPosition.x -= MovementSpeed * Time.deltaTime;
    }

    private bool IsMovingUp() {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || IsNearUpperScreenEdge();
    }

    private bool IsNearUpperScreenEdge() {
        return Input.mousePosition.y >= Screen.height - ReactionSpaceThickness;
    }

    private bool IsMovingDown() {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || IsNearBottomScreenEdge();
    }

    private bool IsNearBottomScreenEdge() {
        return Input.mousePosition.y <= ReactionSpaceThickness;
    }

    private bool IsMovingRight() {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || IsNearRightScreenEdge();
    }

    private bool IsNearRightScreenEdge() {
        return Input.mousePosition.x >= Screen.width - ReactionSpaceThickness;
    }

    private bool IsMovingLeft() {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || IsNearLeftScreenEdge();
    }

    private bool IsNearLeftScreenEdge() {
        return Input.mousePosition.x <= ReactionSpaceThickness;
    }

    private void RestrictMovement() {
        _camPosition.x = Mathf.Clamp(_camPosition.x, -_movementLimit.x, _movementLimit.x);
        _camPosition.y = 10; //arbitrary value greater than 0 but not too big
        _camPosition.z = Mathf.Clamp(_camPosition.z, -_movementLimit.y, _movementLimit.y);
    }

    private void Scroll() {
        _orthographicSize = _cam.orthographicSize;

        AdjustOrthographicSize();
        RestrictScrolling();
        
        _cam.orthographicSize = _orthographicSize;
    }

    private void AdjustOrthographicSize() {
        var scrollMagnitude = Input.GetAxis("Mouse ScrollWheel");
        _orthographicSize -= scrollMagnitude * Time.deltaTime * ScrollSpeed * NormalizationValueForScrollSpeed;
    }

    private void RestrictScrolling() {
        _orthographicSize = Mathf.Clamp(_orthographicSize, MinOrthographicSize, MaxOrthographicSize);
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
        _dragOrigin = _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void MoveCameraWithCursor() {
        _camPosition += _dragOrigin - _cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
