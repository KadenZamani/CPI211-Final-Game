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
        if (player == null) return;

        // 1. Position the camera pivot on the player
        transform.position = player.position + new Vector3(0, offsetDistanceY, 0);

        // 2. Handle Zoom
        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 90); // Added a safety clamp
        }

        // 3. Handle Mouse Input
        if (clickToMoveCamera && Input.GetAxisRaw("Fire2") == 0)
            return;

        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        // 4. Rotate the Camera
        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);

        // 5. OPTION B: Smoothly rotate the player to match the camera's horizontal heading
        // We create a target rotation using only the camera's horizontal (mouseX) value
        Quaternion targetPlayerRotation = Quaternion.Euler(0, mouseX, 0);

        // Slerp (Spherical Linear Interpolation) makes the transition smooth
        player.rotation = Quaternion.Slerp(player.rotation, targetPlayerRotation, Time.deltaTime * playerRotationSpeed);
    }
}