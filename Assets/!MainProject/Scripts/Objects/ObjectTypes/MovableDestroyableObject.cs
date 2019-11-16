using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDestroyableObject : DestroyableObject
{
    public bool isBroken { get; private set; }

    private Rigidbody2D rb;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        type = ObjectType.MovableDestroyable;
        RepairFully();
    }

    public override void DestroyObject()
    {
        base.DestroyObject();
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        isBroken = true;
    }

    protected override void RepairFully()
    {
        base.RepairFully();
        rb.isKinematic = false;
        isBroken = false;
    }
}
