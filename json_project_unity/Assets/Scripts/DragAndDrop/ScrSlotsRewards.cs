using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrSlotsRewards : MonoBehaviour, IDropHandler
{
    public static bool pointerIsOnSlot = false;
    public static string nameSlot;
    public List<string> allName = new List<string>();

    public void OnDrop(PointerEventData eventData)
    {
        nameSlot = name;
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void PointerOnSlot()
    {
        pointerIsOnSlot = true;
    }

    public void PointOutSlot()
    {
        pointerIsOnSlot = false;
    }
}
