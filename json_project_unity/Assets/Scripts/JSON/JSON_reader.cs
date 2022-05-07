using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSON_reader : MonoBehaviour
{
    public TextAsset jsonFile;

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
        Debug.Log(jsonFile.text);
        donneesList = JsonUtility.FromJson<DonneesList>(jsonFile.text);
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
