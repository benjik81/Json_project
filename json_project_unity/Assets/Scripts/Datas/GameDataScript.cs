using UnityEngine;

//allows to add a Unity option to create the GameData object
[CreateAssetMenu(menuName = "Create GameData")]

public class GameDataScript : ScriptableObject
{
    public bool allElementsInScene = false;
    public int count = 0;
    public string playerName = "";
}
