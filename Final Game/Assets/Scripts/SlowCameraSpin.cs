using UnityEngine;

public class SlowCameraSpin : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float degreesPerSecond = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime, Space.World);
    }
}