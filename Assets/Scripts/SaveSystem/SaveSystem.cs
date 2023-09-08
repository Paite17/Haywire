using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem
{
    // saves player data from the player unit
    public static void SavePlayer(Unit playerUnit)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.day";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerUnit);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // loads player data from binary file
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerData.day";

        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    // create a player file to prevent a bug
    public static void CreatePlayerFile(Unit player)
    {
        string path = Application.persistentDataPath + "/playerData.day";
        if (!File.Exists(path))
        {
            Debug.Log("PlayerData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("playerData exists, doing nothing!");
            return;
        }
    }
}
