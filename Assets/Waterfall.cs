using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public GameObject firstgrid;
    private float height;
    private List<GameObject> grids = new();
    public float flowRate;

    void Duplicate(GameObject grid)
    {
        GameObject newgrid = Instantiate(grid, transform);
        newgrid.transform.localPosition = new Vector3(grid.transform.localPosition.x, grid.transform.localPosition.y + height, grid.transform.localPosition.z);
        grids.Add(newgrid);
    }
    void Start()
    {
        height = transform.parent.localScale.y;
        grids.Add(firstgrid);
        Duplicate(grids[0]);
    }
    void FixedUpdate()
    {
        foreach (GameObject grid in grids)
        {
            grid.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y - flowRate, grid.transform.position.z);
        }

        if (grids[0].transform.localPosition.y <= -height)
        {
            Duplicate(grids[^1]);
            Destroy(grids[0]);
            grids.RemoveAt(0);
        }
    }
}
