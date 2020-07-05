using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    
    public GameObject worldMenu;
    public GameObject addMenu;
    private List<GameObject> allApis = new List<GameObject>();
    private List<IDataAPI> apiList = new List<IDataAPI>();
    private List<GameObject> btnList = new List<GameObject>();
    private List<DataObject[][]> saveDataList = new List<DataObject[][]>();
    private ActivateVizualizer activateVisualizer;
    private GameObject infoButton;

    private void Start() {
     activateVisualizer = GetComponent<ActivateVizualizer>();  
    }
    public void addButton(GameObject btn)
    {
        allApis.Add(btn);
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        button.GetComponent<ScrollButtonButton>().setDeletable(true);
        Destroy(button.GetComponent<ItemDragHandler>());
        apiList.Add(button.GetComponent<IDataAPI>());
        btnList.Add(button);
        ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
        DateTime localDate = DateTime.Now;
        DateTime currentDate = GameObject.Find("WorldMenuManager").GetComponent<WorldMenuBehaviour>().getCurrentDate();
        if(DateTime.Equals(localDate.Date, currentDate.Date)) {
            DataObject[] data = button.GetComponent<IDataAPI>().simpleRequest();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data);
        } else {
            DataObject[][] currentData;
            if (worldMenu.GetComponent<WorldMenuBehaviour>().timeLapseIsOn)
            { 
                DateTime start = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("start"));
                DateTime end = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("end"));
                currentData = button.GetComponent<IDataAPI>().dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
                saveDataList.Add(currentData);
                
            } else {
            currentData = button.GetComponent<IDataAPI>().dateRequest(currentDate.ToString("yyyy-MM-dd"), currentDate.ToString("yyyy-MM-dd"));
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);

            }
        }
    }

    public void deleteAPI() {
        if(infoButton != null) {

        apiList.Remove(infoButton.GetComponent<IDataAPI>());
        Debug.Log(apiList.Count);
        btnList.Remove(infoButton);
        Debug.Log(btnList.Count);
        foreach (Transform child in addMenu.transform)
        {
            if(infoButton.tag.Equals(child.gameObject.tag)) {
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        Destroy(infoButton);
        infoButton = null;
        }
    }

    public void saveTimeSpanData(DateTime start, DateTime end) {
        activateVisualizer.deleteDrawings();

        foreach(GameObject button in btnList)
        {
            DataObject[][] currentData = button.GetComponent<IDataAPI>().dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
            saveDataList.Add(currentData);
            ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
        }
    }

    public void visualizeTimespanData(float index) {
         activateVisualizer.deleteDrawings();
         GameObject[] buttons = btnList.ToArray();
         int counter = 0;
         foreach (DataObject[][] data in saveDataList)
         {
             ScrollButtonButton buttonScript = buttons[counter].GetComponent<ScrollButtonButton>();
             activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data, (int)index);
             counter++;
         }

    }

    public void setActiveBtn(GameObject button) {
        infoButton = button;
    }


    public List<GameObject> getBtnList()
    {
        return btnList;
    }

    public void setBtnList(List<GameObject> newList)
    {
        btnList = newList;
    }

    public List<IDataAPI> getApiList()
    {
        return apiList;
    }

    public void setApiList(List<IDataAPI> newList)
    {
        apiList = newList;
    }
}
