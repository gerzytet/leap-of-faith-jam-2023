using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool flippedLeft;
    public Color activeColor;
    public Color inactiveColor;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponentInParent<Frog>() != null)
        {
            col.gameObject.GetComponentInParent<Frog>().SetCheckpoint(this);
        }
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().color =
            Frog.instance.currentCheckpoint == this ? activeColor : inactiveColor;
    }
}
