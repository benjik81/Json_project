using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scrEndRewardsBehavior : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Vector2 initialPosition;
    public static string nameDrag;
    public GameObject JSON_reader;
    public Canvas canvas;
    private List<string> allName;
    [SerializeField] private GameDataScript GameData;

    void Start()
    {
        initialPosition = rectTransform.anchoredPosition;
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        nameDrag = name;
        JSON_reader = Instantiate(JSON_reader);
        JSON_reader.transform.SetParent(GameObject.Find("ReaderContainer").transform);
        allName = GetAllName(JSON_reader);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        int id = GetID(allName, name);
        rectTransform.sizeDelta = new Vector2(JSON_reader.GetComponent<JSON_reader>().donneesList.donnees[id].trueDimension.trueWidth,
            JSON_reader.GetComponent<JSON_reader>().donneesList.donnees[id].trueDimension.trueHeight);
        Debug.Log("Z : " + JSON_reader.GetComponent<JSON_reader>().GetRotation(id+1).rZ);
        rectTransform.rotation = Quaternion.Euler(
            JSON_reader.GetComponent<JSON_reader>().GetRotation(id + 1).rX,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(id + 1).rY,
            JSON_reader.GetComponent<JSON_reader>().GetRotation(id + 1).rZ
            );
        Debug.Log("Test : " + rectTransform.rotation);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        int id = GetID(allName, name);
        Debug.Log("Name slot : " + ScrSlotsRewards.nameSlot + "\n Name object : " + name + "\n TEST : " + name + "Slot");
        if(ScrSlotsRewards.nameSlot != name + "Slot")
        {
            GameData.nbErrors++;
            rectTransform.anchoredPosition = initialPosition;
            canvasGroup.blocksRaycasts = true;
            rectTransform.sizeDelta = new Vector2(JSON_reader.GetComponent<JSON_reader>().donneesList.donnees[id].dimension.width, JSON_reader.GetComponent<JSON_reader>().donneesList.donnees[id].dimension.height);
            rectTransform.rotation = Quaternion.Euler(
                JSON_reader.GetComponent<JSON_reader>().GetRotation(id).rX,
                JSON_reader.GetComponent<JSON_reader>().GetRotation(id).rY,
                JSON_reader.GetComponent<JSON_reader>().GetRotation(id).rZ
                );
        }
        else
        {
            GameData.count += 1;
            if (GameData.count == JSON_reader.GetComponent<JSON_reader>().donneesList.donnees.Length / 2)
            {
                GameData.allElementsInScene = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public List<string> GetAllName(GameObject reader)
    {
        List<string> list = new List<string>();
        for(int i = 0; i < reader.GetComponent<JSON_reader>().donneesList.donnees.Length; i++)
        {
            list.Add(reader.GetComponent<JSON_reader>().donneesList.donnees[i].name);
        }
        return list;
    }

    public int GetID(List<string> aN, string name)
    {
        int id = 0;
        for(int i = 0; i < aN.Count; i++)
        {
            if (aN[i] == name)
                id = i;
        }
        return id;
    }
}
