using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private Vector3 sleepPoint;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public float moveSpeedMultiplier;

    private Rigidbody2D rb;
    private Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeedMultiplier = 1f;
    }

    private void Start()
    {
        sleepPoint = transform.position;
    }

    private void Update()
    {
        if (canMove)
        {
            inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();

            anim.SetBool("walkRun", inputVector != Vector2.zero);
        }

        if (inputVector != Vector2.zero)
            visualRoot.localScale = new Vector3(Mathf.Sign(inputVector.x), 1, 1);
    }

    public void GoSleep()
    {
        StartCoroutine(GoSleepCoroutine());
    }

    public void GoAwakeStopSleep()
    {
        StartCoroutine(StopSleepCoroutine());
    }

    private IEnumerator StopSleepCoroutine()
    {
        yield return new WaitForSeconds(1f);
        canMove = true;
        anim.SetBool("sleep", false);
    }

    private IEnumerator GoSleepCoroutine()
    {
        canMove = false;
        inputVector = Vector2.zero;
        anim.SetBool("walkRun", true);
        Vector3 moveDir = sleepPoint - transform.position;
        moveDir.Normalize();
        visualRoot.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
        while (Vector2.Distance(transform.position, sleepPoint) > 0.3f)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            yield return null;
        }
        anim.SetBool("walkRun", false);
        anim.SetBool("sleep", true);
    }

    private void FixedUpdate()
    {
        rb.velocity = inputVector * moveSpeed * moveSpeedMultiplier;
    }
}
