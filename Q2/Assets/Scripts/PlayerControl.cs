using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;
    public float moveSpeed = 0f;
    public float jumpForce = 0f;
    private bool isGrounded = false;
    private bool jumpQueued = false;
    private bool started = false;
    public GameObject startButton;
    public GameObject powerText;
    public GameObject winText;
    public Text countText;
    public Color powerColor = Color.magenta;
    public float speedBoost = 2f;
    private GameObject powerUp;
    private bool powered = false;
    private int count = 0;
    private int total;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        total = GameObject.FindGameObjectsWithTag("PickUp").Length;
        countText.text = "Coins: " + count + " / " + total;
        powerUp = GameObject.FindWithTag("PowerUp");
        powerUp.SetActive(false);
    }

    public void StartGame()
    {
        started = true;
        rb.useGravity = true;
        startButton.SetActive(false);
    }

    void Update()
    {
        if (started && isGrounded && jumpAction.triggered)
        {
            jumpQueued = true;
        }
    }

    private void FixedUpdate()
    {
        if (!started) return;
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(-moveValue.x, 0f, -moveValue.y);
        rb.AddForce(movement * moveSpeed);
        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpQueued = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
        if (powered && collision.gameObject.CompareTag("BreakableWall"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            countText.text = "Coins: " + count + " / " + total;
            if (count == total)
            {
                powerUp.SetActive(true);
            }
        }
        if (other.CompareTag("PowerUp"))
        {
            other.gameObject.SetActive(false);
            powered = true;
            moveSpeed *= speedBoost;
            GetComponent<Renderer>().material.color = powerColor;
            powerText.SetActive(true);
        }
        if (other.CompareTag("Goal"))
        {
            winText.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
