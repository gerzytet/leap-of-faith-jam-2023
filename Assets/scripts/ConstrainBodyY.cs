/*
@Authors - Craig
@Description - Y movement constraints
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainBodyY : MonoBehaviour
{
    [SerializeField] private Transform foot;

    public float distance = 0.5f;

    void FixedUpdate()
    {
        if (transform.position.y - foot.transform.position.y < distance)
        {
            transform.position = new Vector3(transform.position.x,
                foot.transform.position.y + distance, transform.position.z);
        }
    }
}
