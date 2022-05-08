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
    public GameObject JSON_backgrounds_reader;

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

    [Header("Backgrounds")]
    public GameObject connexionBackground;
    public GameObject gameBackground;
    public GameObject menuBackground;
    public GameObject endBackground;

    [Header("Buttons Backgrounds")]
    public GameObject exitButton;
    public GameObject validateButton;
    public GameObject playButton;
    public GameObject exitMenuButton;
    public GameObject backButton;
    public GameObject endGameExitButton;
    
    private bool isValid = false;
    private bool isLoad = false;
    private PlayerDico data;
    private Player player;

    void Start()
    {
        int limit = JSON_reader.GetComponent<JSON_reader>().donneesList.donnees.Length;
        gameData.playerName = "";
        gameData.count = 0;
        gameData.allElementsInScene = false;
        gameData.nbErrors = 0;
        isValid = false;
        for (int i = 0; i < limit; i++)
        {
            if(JSON_reader.GetComponent<JSON_reader>().GetType(i) == "Slot")
            {
                createElements(slotPrefab, "Slots", i);
            }
            if(JSON_reader.GetComponent<JSON_reader>().GetType(i) == "Item")
            {
                createElements(itemPrefab, "Items", i);
            }
        }

        int limit_backgrounds = JSON_backgrounds_reader.GetComponent<JSON_background_Reader>().backgroundsList.backgrounds.Length;
        Debug.Log("Nombre de PNG : " + limit_backgrounds);
        for(int j = 0; j < limit_backgrounds; j++)
        {
            CreateBackgrounds(j);
        }
    }

    //Function allowing to generate the items and slots of the scene whose data come from a JSON file.
    public void createElements(GameObject prefab, string name, int indice)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(GameObject.Find(name).transform);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(JSON_reader.GetComponent<JSON_reader>().GetPosition(indice).x, JSON_reader.GetComponent<JSON_reader>().GetPosition(indice).y);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).width, JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).height);
        go.GetComponent<Image>().sprite = CreateSprite(indice);
        //Resources.Load<Sprite>(JSON_reader.GetComponent<JSON_reader>().GetSourceImage(indice));
        go.name = JSON_reader.GetComponent<JSON_reader>().GetName(indice);
        go.GetComponent<RectTransform>().rotation = Quaternion.Euler(
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rX,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rY,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(indice).rZ
            );
    }

    public void CreateBackgrounds(int indice)
    {
        Debug.Log("TYPE : " + JSON_backgrounds_reader.GetComponent<JSON_background_Reader>().backgroundsList.backgrounds[indice].type);
        switch (JSON_backgrounds_reader.GetComponent<JSON_background_Reader>().backgroundsList.backgrounds[indice].type)
        {
            case "connexion_background":
                Debug.Log("CONNEXION BACKGROUND");
                connexionBackground.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, connexionBackground);
                break;
            case "buttons_background":
                exitButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, exitButton);
                validateButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, validateButton);
                playButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, playButton);
                exitMenuButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, exitMenuButton);
                endGameExitButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, endGameExitButton);
                break;
            case "all_background":
                menuBackground.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, menuBackground);
                gameBackground.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, gameBackground);
                break;
            case "end_background":
                endBackground.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, endBackground);
                break;
            case "back_background":
                backButton.GetComponent<Image>().sprite = CreateBackgroundSprite(indice, backButton);
                break;
        }
    }

    public Sprite CreateBackgroundSprite(int indice, GameObject background)
    {
        float width = background.GetComponent<RectTransform>().sizeDelta.x;
        float height = background.GetComponent<RectTransform>().sizeDelta.y;
        string pathImage = Application.streamingAssetsPath + "/" + JSON_backgrounds_reader.GetComponent<JSON_background_Reader>().backgroundsList.backgrounds[indice].imagesource + ".png";
        byte[] pngBytes = System.IO.File.ReadAllBytes(pathImage);
        Texture2D tex = new Texture2D((int)width, (int)height);
        tex.LoadImage(pngBytes);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(1f, 1f));
    }

    public Sprite CreateSprite(int indice)
    {
        int width = JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).width;
        int height = JSON_reader.GetComponent<JSON_reader>().GetDimension(indice).height;
        string pathImage = Application.streamingAssetsPath + "/" + JSON_reader.GetComponent<JSON_reader>().GetSourceImage(indice) + ".png";
        byte[] pngBytes = System.IO.File.ReadAllBytes(pathImage);
        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(pngBytes);
        Sprite result = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(1f, 1f));
        return result;
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
        gameData.nbErrors = 0;
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
            data.joueurs[play.name].nbErrors = gameData.nbErrors;
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
        j.nbErrors = gameData.nbErrors;
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
        if(player.count != JSON_reader.GetComponent<JSON_reader>().donneesList.donnees.Length/2 && player.count != 0)
        {
            gameData.count = 0;
        }
        else
        {
            gameData.count = player.count;
        }
        gameData.playerName = player.name;
        gameData.nbErrors = player.nbErrors;
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
        data = JsonConvert.DeserializeObject<PlayerDico>(jsonFile);
        displayData(data);
    }

    public void ResetGame()
    {
        //We reset the gameData.
        gameData.allElementsInScene = false;
        gameData.playerName = "";
        gameData.count = 0;
        gameData.nbErrors = 0;
        userName.text = "";
    }

    public void displayData(PlayerDico d)
    {
        foreach(KeyValuePair<string, Player> kvp in d.joueurs)
        {
            Debug.Log(kvp.Value.name);
        }
    }
}
