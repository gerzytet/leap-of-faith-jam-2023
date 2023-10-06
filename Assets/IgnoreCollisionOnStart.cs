using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionOnStart : MonoBehaviour
{
    public Collider2D thisCollider;
    public Collider2D other;
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other);
    }
}
