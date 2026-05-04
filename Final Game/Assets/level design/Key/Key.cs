using UnityEngine;

public class Key : MonoBehaviour
{
    public float rotateSpeed = 90f;      // degrees per second
    public float bounceHeight = 0.25f;   // vertical movement amount
    public float bounceSpeed = 2f;       // bounce cycles per second

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // spin
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);

        // bounce
        float offsetY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPosition + new Vector3(0f, offsetY, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterStateController player =
            other.GetComponent<CharacterStateController>();

        if (player != null)
        {
            player.addKey();
            Destroy(gameObject);
        }
    }
}
