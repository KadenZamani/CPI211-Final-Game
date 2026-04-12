using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Enable to move the camera by holding the right mouse button.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel.")]
    public bool canZoom = true;
    [Space]
    public float sensitivity = 5f;

    [Tooltip("How fast the player snaps to face the camera direction.")]
    public float playerRotationSpeed = 10f;

    [Tooltip("Camera Y rotation limits.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);
    public bool isLocked = false;
    float mouseX;
    float mouseY;
    float offsetDistanceY;

    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetDistanceY = transform.position.y;

        // Initialize mouseX so the camera starts at the player's current rotation
        mouseX = player.eulerAngles.y;

        if (!clickToMoveCamera)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Using LateUpdate for cameras prevents stuttering
    void LateUpdate()
    {
        // 1. Check if the player is missing OR if the camera is locked
        if (player == null || isLocked)
        {
            // If the player died, we might want to unlock the cursor so they can click menus
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // --- All movement logic below remains the same ---

        transform.position = player.position + new Vector3(0, offsetDistanceY, 0);

        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 90);
        }

        if (clickToMoveCamera && Input.GetAxisRaw("Fire2") == 0)
            return;

        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);

        Quaternion targetPlayerRotation = Quaternion.Euler(0, mouseX, 0);
        player.rotation = Quaternion.Slerp(player.rotation, targetPlayerRotation, Time.deltaTime * playerRotationSpeed);
    }
}