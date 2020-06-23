using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    private RectTransform dataMenu;
    public void OnDrop(PointerEventData eventData) {
        dataMenu = transform as RectTransform;
        Debug.Log("Drop");
        if(RectTransformUtility.RectangleContainsScreenPoint(dataMenu, Input.mousePosition)) {
            Debug.Log("Item Drop!");
            if(eventData.pointerDrag != null) {
                GetComponent<ScrollButtonControl>().addButton(eventData.pointerDrag);
                eventData.pointerDrag.SetActive(false);
            }
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
