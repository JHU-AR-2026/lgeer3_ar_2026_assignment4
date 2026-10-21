using UnityEngine;

public class Bob : MonoBehaviour
{
    public float height = 0.25f;
    public float speed = 2f;
    private Vector3 start;

    void Start()
    {
        start = transform.position;
    }

    void Update()
    {
        transform.position = start + Vector3.up * height * Mathf.Sin(Time.time * speed);
    }
}
