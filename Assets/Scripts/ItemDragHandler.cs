using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 position = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("StartDrag");
        position = transform.localPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<Image>().maskable = false;
        
    }
    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("EndDrag");
         GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        transform.localPosition = position;
        GetComponent<Image>().maskable = true;
    } 
   
}
