using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollButtonControl : MonoBehaviour, ISelecionChangeObserver
{
    [SerializeField]
    private GameObject worldMenu;
    [SerializeField]
    private GameObject addMenu;
    [SerializeField]
    private GameObject parent;

    private string currentKey;
    private Dictionary<string, List<IDataAPI>> apiList = new Dictionary<string, List<IDataAPI>>();
    private Dictionary<string, List<GameObject>> btnList = new Dictionary<string, List<GameObject>>();
    private ActivateVizualizer activateVisualizer;
    private List<DataObject[][]> saveDataList = new List<DataObject[][]>();
    private GameObject infoButton;

    private void Awake()
    {
        worldMenu.GetComponent<WorldMenuBehaviour>().AddSelectionChangeObserver(this);
        activateVisualizer = GetComponent<ActivateVizualizer>();
    }

    public void onChange(GameObject selected)
    {
        if (currentKey != null && currentKey != selected.name)
        {
            foreach (GameObject btn in getBtnList())
            {
                btn.SetActive(false);
            }
            currentKey = selected.name;
            List<string> active = new List<string>();
            foreach (GameObject btn in getBtnList())
            {
                btn.SetActive(true);
                active.Add(btn.GetComponent<ScrollButtonButton>().key);
            }
            foreach (Transform original in addMenu.transform)
            {
                bool enabled = active.Contains(original.GetComponent<ScrollButtonButton>().key);
                original.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                original.gameObject.SetActive(!enabled);
            }
        }
        else
        {
            currentKey = selected.name;
        }
    }

    public void addButton(GameObject btn)
    {
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        button.GetComponent<ScrollButtonButton>().setDeletable(true);
        Destroy(button.GetComponent<ItemDragHandler>());
        getApiList().Add(button.GetComponent<IDataAPI>());
        getBtnList().Add(button);
        ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
        DateTime localDate = DateTime.Now;
        DateTime currentDate = GameObject.Find("WorldMenuManager").GetComponent<WorldMenuBehaviour>().getCurrentDate();
        if (DateTime.Equals(localDate.Date, currentDate.Date))
        {
            DataObject[] data = button.GetComponent<IDataAPI>().simpleRequest();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data);
        }
        else
        {
            DataObject[][] currentData;
            if (worldMenu.GetComponent<WorldMenuBehaviour>().timeLapseIsOn)
            {
                DateTime start = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("start"));
                DateTime end = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("end"));
                currentData = button.GetComponent<IDataAPI>().dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
                saveDataList.Add(currentData);
            }
            else
            {
                currentData = button.GetComponent<IDataAPI>().dateRequest(currentDate.ToString("yyyy-MM-dd"), currentDate.ToString("yyyy-MM-dd"));
                activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
            }
        }
    }

    public void drawSingleDay(DateTime date) {
        foreach (GameObject button in getBtnList())
        {
            activateVisualizer.deleteDrawings();
            ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
            DataObject[][] currentData = button.GetComponent<IDataAPI>().dateRequest(date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
            
        }
    }

    public void deleteAPI()
    {
        if (infoButton != null)
        {
            getApiList().Remove(infoButton.GetComponent<IDataAPI>());
            getBtnList().Remove(infoButton);
            foreach (Transform child in addMenu.transform)
            {
                if (infoButton.tag.Equals(child.gameObject.tag))
                {
                    child.gameObject.SetActive(true);
                    child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
            activateVisualizer.deleteVisual(infoButton.GetComponent<ScrollButtonButton>().key);
            Destroy(infoButton);
            infoButton = null;
        }
    }

    public void saveTimeSpanData(DateTime start, DateTime end)
    {
        activateVisualizer.deleteDrawings();
        foreach (GameObject button in getBtnList())
        {
            DataObject[][] currentData = button.GetComponent<IDataAPI>().dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
            saveDataList.Add(currentData);
            ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
        }
    }

    public void visualizeTimespanData(float index)
    {
        activateVisualizer.deleteDrawings();
        GameObject[] buttons = getBtnList().ToArray();
        int counter = 0;
        foreach (DataObject[][] data in saveDataList)
        {
            ScrollButtonButton buttonScript = buttons[counter].GetComponent<ScrollButtonButton>();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data, (int)index);
            counter++;
        }
    }

    public void setActiveBtn(GameObject button)
    {
        infoButton = button;
    }

    public List<GameObject> getBtnList()
    {
        return btnList.GetOrCreate(currentKey);
    }

    public void setBtnList(List<GameObject> newList)
    {
        btnList[currentKey] = newList;
    }

    public List<IDataAPI> getApiList()
    {
        return apiList.GetOrCreate(currentKey);
    }

    public void setApiList(List<IDataAPI> newList)
    {
        apiList[currentKey] = newList;
    }
}
