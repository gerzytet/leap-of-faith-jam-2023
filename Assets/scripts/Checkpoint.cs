/*
@Authors - Craig and Patrick
@Description - Handles checkpoints
*/

using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool flippedLeft;
    public Color activeColor;
    public Color inactiveColor;
    public bool isStartOfLevel;
    public string startOfLevelMessage;
    private bool touched = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponentInParent<Frog>() != null)
        {
            col.gameObject.GetComponentInParent<Frog>().SetCheckpoint(this);
        }

        if (!touched && isStartOfLevel)
        {
            SignOverlay.instance.DisplayStartOfLevel(name.Substring(0, name.IndexOf(' ', 6) + 1), startOfLevelMessage);
            touched = true;
        }
    }

    void Update()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().color =
                Frog.instance.currentCheckpoint == this ? activeColor : inactiveColor;
        }
    }
}
