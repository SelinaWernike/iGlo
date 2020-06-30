using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private List<IDataAPI> apiList = new List<IDataAPI>();
    private List<GameObject> btnList = new List<GameObject>();
   
    public void addButton(GameObject btn) {
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform,false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(button.GetComponent<ItemDragHandler>());
        apiList.Add(button.GetComponent<IDataAPI>());
        btnList.Add(button);
        GetComponent<ActivateVizualizer>().visualize(button.GetComponent<IDataAPI>().simpleRequest());
    }

    public List<GameObject> getBtnList() {
        return btnList;
    }

    public void setBtnList(List<GameObject> newList) {
        btnList = newList;
    }

    public List<IDataAPI> getApiList() {
        return apiList;
    }

    public void setApiList(List<IDataAPI> newList) {
        apiList = newList;
    }
}
