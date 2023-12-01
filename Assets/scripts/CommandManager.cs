using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static bool commandsEnabled = true;
    public TextMeshProUGUI command;
    private string commandBuffer = "";
    
    void Update()
    {
        command.enabled = commandsEnabled;
        if (!commandsEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                //process command
                if (commandBuffer.StartsWith("/tp "))
                {
                    string args = commandBuffer.Substring(4);
                    Debug.Log(args + args.Length);
                    if (args.Contains(" "))
                    {
                        string[] parts = args.Split(' ');
                        if (parts.Length == 2)
                        {
                            float x = float.Parse(parts[0]);
                            float y = float.Parse(parts[1]);
                            Frog.instance.transform.position = new Vector3(x, y, Frog.instance.transform.position.z);
                        }
                    }
                    else if (args.StartsWith("f") && args.Length == 2)
                    {
                        int num = Int32.Parse(args.Substring(1));
                        Debug.Log("did f");
                        Frog.instance.currentLevel = num;
                        Frog.instance.TeleportToLevel();
                    }
                }
            }
            finally
            {
                commandBuffer = "";
            }
        }
        else
        {
            string inputString = Input.inputString;
            if (commandBuffer.Length > 0 || inputString.StartsWith("/"))
            {
                commandBuffer += inputString;
            }
        }
        command.text = commandBuffer;
    }
}
