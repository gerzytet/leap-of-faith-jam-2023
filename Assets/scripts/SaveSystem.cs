using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void savePlayer(Frog f){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(f);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayer() {
        string path = Application.persistentDataPath + "/player.txt";

        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //c-sharp casting
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;

        } else{
            Debug.Log("Save file not found");
            return null;
        }
    }
}