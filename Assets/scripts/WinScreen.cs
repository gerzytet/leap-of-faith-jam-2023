using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI t;
    
    // Start is called before the first frame update
    void Start()
    {
        t.text = "Final Time: " + Timer.instance.currentTime.ToString("0.00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
