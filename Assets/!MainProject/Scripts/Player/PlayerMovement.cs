using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float raycastDistance;

    [HideInInspector] public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
    }

    private void FixedUpdate()
    {
        //if (inputVector != Vector2.zero)
            //CheckForMovableObjects();

        rb.velocity = inputVector * moveSpeed;
    }

    private void CheckForMovableObjects()
    {
        Direction moveDir = Direction.Down;
        Transform hitted = null;

        if (inputVector.x != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * inputVector.x, raycastDistance, GameManager.instance.movableObjects);
            if (hit)
            {
                moveDir = Mathf.Sign(inputVector.x) > 0 ? Direction.Right : Direction.Left;
                hitted = hit.transform;
            }
        }

        if (inputVector.y != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * inputVector.y, raycastDistance, GameManager.instance.movableObjects);
            if (hit)
            {
                moveDir = Mathf.Sign(inputVector.y) > 0 ? Direction.Up : Direction.Down;
                hitted = hit.transform;
            }
        }

        if (hitted != null)
        {
            hitted.GetComponent<MovableObject>().Move(moveDir, moveSpeed);
        }
    }
}
