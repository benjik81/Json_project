using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSON_background_Reader : MonoBehaviour
{
    //public TextAsset jsonFile;
    private string jsonFile;

    [System.Serializable]
    public class Background
    {
        public string name;
        public string type;
        public string imagesource;
    }

    [System.Serializable]
    public class BackgroundList
    {
        public Background[] backgrounds;
    }

    public BackgroundList backgroundsList = new BackgroundList();

    private void Awake()
    {
        string backgroundFilePath = Application.streamingAssetsPath + "/JSON/donnees_backgrounds.json";
        jsonFile = File.ReadAllText(backgroundFilePath);
        backgroundsList = JsonUtility.FromJson<BackgroundList>(jsonFile);
    }
}
