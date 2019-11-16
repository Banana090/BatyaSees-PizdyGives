using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float moveSpeed;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public float moveSpeedMultiplier;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        moveSpeedMultiplier = 1f;
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();

        anim.SetBool("walkRun", inputVector != Vector2.zero);

        if (inputVector != Vector2.zero)
            visualRoot.localScale = new Vector3(Mathf.Sign(inputVector.x), 1, 1);
    }

    private void FixedUpdate()
    {
        rb.velocity = inputVector * moveSpeed * moveSpeedMultiplier;
    }
}
