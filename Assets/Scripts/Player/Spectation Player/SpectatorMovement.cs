using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float moveSpeed;
    
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float horizontalMovement;
    private float verticalMovement;

    [Header("Movement")]
    [SerializeField] private Transform hightCheck;
    [SerializeField] private float minHight;
    [SerializeField] private float maxHight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveInput();
    }

    private void MoveInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if (Input.GetKey(KeyCode.LeftShift) && hightCheck.position.y > minHight)
        {
            moveDirection += Vector3.down;
        }

        if (Input.GetKey(KeyCode.Space) && hightCheck.position.y < maxHight)
        {
            moveDirection += Vector3.up;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb.drag = 4;
        if (moveDirection == Vector3.zero)
        {
            rb.drag = Mathf.Lerp(1, 40, 0.1f);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
        }
        moveDirection = Vector3.zero;
    }
}
