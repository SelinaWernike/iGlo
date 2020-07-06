using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    private RectTransform dataMenu;
    public async void OnDrop(PointerEventData eventData) {
        dataMenu = transform as RectTransform;
        if(RectTransformUtility.RectangleContainsScreenPoint(dataMenu, Input.mousePosition)) {
            if(eventData.pointerDrag != null) {
                GameObject target = eventData.pointerDrag;
                await GetComponent<ScrollButtonControl>().addButton(eventData.pointerDrag);
                target.SetActive(false);
            }
        }
    }

  
}
