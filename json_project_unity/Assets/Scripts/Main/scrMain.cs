using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scrMain : MonoBehaviour
{
    [Header("Json")]
    public GameObject JSON_reader;

    [Header("Username zone")]
    [SerializeField] private TMP_InputField userName;

    [Header("Prefabs")]
    public GameObject itemPrefab;
    public GameObject slotPrefab;

    [Header("Datas")]
    [SerializeField] private GameDataScript gameData;
    
    [Header("Scenes")]
    public GameObject endGameScene;
    public GameObject gameScene;
    public GameObject menuScene;
    
    private bool isValid = false;
    private bool isLoad = false;
    private Donnees data;
    private Player player;

    void Start()
    {
        int limit = JSON_reader.GetComponent<JSON_reader>().donneesList.donnees.Length;
        gameData.playerName = "";
        gameData.count = 0;
        gameData.allElementsInScene = false;
        isValid = false;
        for(int i = 0; i < limit; i++)
        {
            if(JSON_reader.GetComponent<JSON_reader>().GetType(i) == "Slot")
            {
                createElements(slotPrefab, "Slots", i);
            }
            else
            {
                createElements(itemPrefab, "Items", i);
            }
        }
    }

    //Function allowing to generate the items and slots of the scene whose data come from a JSON file.
    public void createElements(GameObject prefab, string name, int indice)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(GameObject.Find(name).transform);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(JSON_reader.GetComponent<JSON_reader>().GetPosition(indice).x, JSON_reader.GetComponent<JSON_reader>().GetPosition(indice).y);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).width, JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).height);
        go.GetComponent<Image>().sprite = Resources.Load<Sprite>(JSON_reader.GetComponent<JSON_reader>().GetSourceImage(indice));
        go.name = JSON_reader.GetComponent<JSON_reader>().GetName(indice);
        go.GetComponent<RectTransform>().rotation = Quaternion.Euler(
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rX,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rY,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rZ
            );
    }

    void Update()
    {
        //If all the elements have been placed then we go to the End Scene.
        if (gameData.count == JSON_reader.GetComponent<JSON_reader>().donneesList.donnees.Length / 2 && !isValid)
        {
            endGameScene.SetActive(true);
            gameScene.SetActive(false);
            menuScene.SetActive(false);
            isValid = true;
        }
    }
    
    public void ExitGame()
    {
        //We store the data in the backup json and we reset the GameData.
        WriteInJson();
        gameData.count = 0;
        gameData.allElementsInScene = false;
        gameData.playerName = "";
        Application.Quit();
    }

    public void QuitGame()
    {
        //In case the player leaves without logging in, there is nothing to do but quit the application.
        Application.Quit();
    }

    public void WriteInJson()
    {
        //We get the path of the file and we write the data contained in the GameData.
        string path = Application.persistentDataPath + "/savedatas.json";
        Debug.Log("Path : " + path);
        Debug.Log(gameData.playerName + gameData.count + gameData.allElementsInScene);
        Player play = CreationJoueur();
        Debug.Log(play.name + play.count + play.allElementsInScene);
        Debug.Log("EXISTE ? : " + play.name + " : " + data.joueurs.ContainsKey(play.name));
        if (data.joueurs.ContainsKey(play.name))
        {
            Debug.Log("Le joueur existe déjà !");
            data.joueurs[play.name].count = gameData.count;
            data.joueurs[play.name].allElementsInScene = gameData.allElementsInScene;
        }
        else
        {
            Debug.Log("Nouveau joueur !");
            data.joueurs.Add(play.name, play);
        }
        string json = JsonConvert.SerializeObject(data);
        Debug.Log("JSON : " + json);
        File.WriteAllText(path,json);
    }

    public Player CreationJoueur()
    {
        //A player corresponds to the data that will be saved in the JSON, the data is basically contained in the GameData (ScriptableObject)
        Player j = new Player();
        j.name = gameData.playerName;
        j.count = gameData.count;
        j.allElementsInScene = gameData.allElementsInScene;
        return j;
    }

    public Player GetPlayer()
    {
        Debug.Log("EXISTE ? " + data.joueurs.ContainsKey(gameData.playerName));
        //We look at the launch if the player exists to recover or not his data.
        if (data.joueurs.ContainsKey(gameData.playerName))
        {
            return data.joueurs[gameData.playerName];
        }

        return CreationJoueur();
    }

    public void LoadPlayer()
    {
        //The gameData takes the data of the corresponding player stored in the JSON
        player = GetPlayer();
        Debug.Log(player.name + player.allElementsInScene + player.count);
        gameData.allElementsInScene = player.allElementsInScene;
        gameData.count = player.count;
        gameData.playerName = player.name;
    }

    public void SetName()
    {
        gameData.playerName = userName.text;
        Debug.Log("CONNEXION DU JOUEUR : " + gameData.playerName);
        InitJson();
        LoadPlayer();
    }

    public void InitJson()
    {
        //Here, if the JSON backup file does not exist, we create it. 
        //Then our "data" dictionary will retrieve all the data contained in the JSON, 
        //that is, all the players and their variables.
        string path = Application.persistentDataPath + "/savedatas.json";
        
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "{}");
        }

        string jsonFile = File.ReadAllText(path);
        Debug.Log("JsonFile : " + jsonFile);
        //gameData = JsonConvert.DeserializeObject<GameDataScript>(jsonFile);
        data = JsonConvert.DeserializeObject<Donnees>(jsonFile);
        displayData(data);
    }

    public void ResetGame()
    {
        //We reset the gameData.
        gameData.allElementsInScene = false;
        gameData.playerName = "";
        gameData.count = 0;
        userName.text = "";
    }

    public void displayData(Donnees d)
    {
        foreach(KeyValuePair<string, Player> kvp in d.joueurs)
        {
            Debug.Log(kvp.Value.name);
        }
    }
}
