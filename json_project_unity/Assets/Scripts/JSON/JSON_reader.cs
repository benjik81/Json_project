using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSON_reader : MonoBehaviour
{
    //public TextAsset jsonFile;
    private string jsonFile;

    [System.Serializable]
    public class Donnees
    {
        public string name;
        public string type;
        public Position position;
        public Dimension dimension;
        public string imagesource;
        public Rotation rotation;
        public TrueDimension trueDimension;
    }

    [System.Serializable]
    public class Position
    {
        public int x, y;
    }

    [System.Serializable]
    public class Dimension
    {
        public int width, height;
    }

    [System.Serializable]
    public class Rotation
    {
        public int rX, rY, rZ;
    }

    [System.Serializable]
    public class TrueDimension
    {
        public int trueWidth, trueHeight;
    }

    [System.Serializable]
    public class DonneesList
    {
        public Donnees[] donnees;
    }


    public DonneesList donneesList = new DonneesList();

    // Start is called before the first frame update
    void Awake()
    {
        string backgroundFilePath = Application.streamingAssetsPath + "/JSON/donnees.json";
        jsonFile = File.ReadAllText(backgroundFilePath);
        donneesList = JsonUtility.FromJson<DonneesList>(jsonFile);
    }

    public string GetName(int pos)
    {
        return donneesList.donnees[pos].name;
    }

    public string GetType(int pos)
    {
        return donneesList.donnees[pos].type;
    }

    public Position GetPosition(int pos)
    {
        return donneesList.donnees[pos].position;
    }

    public Dimension GetDimension(int pos)
    {
        return donneesList.donnees[pos].dimension;
    }

    public string GetSourceImage(int pos)
    {
        return donneesList.donnees[pos].imagesource;
    }

    public Rotation GetRotation(int pos)
    {
        return donneesList.donnees[pos].rotation;
    }

    public TrueDimension GetTrueDimension(int pos)
    {
        return donneesList.donnees[pos].trueDimension;
    }
}
