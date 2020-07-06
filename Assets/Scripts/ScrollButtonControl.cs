using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class ScrollButtonControl : MonoBehaviour, ISelecionChangeObserver
{
    [SerializeField]
    private GameObject worldMenu;
    [SerializeField]
    private GameObject addMenu;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private GameObject loadingIcon;

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

    public async Task addButton(GameObject btn)
    {
        loadingIcon.SetActive(true);
        ScrollButtonButton buttonScript = btn.GetComponent<ScrollButtonButton>();
        IDataAPI dataAPI = btn.GetComponent<IDataAPI>();
        DateTime localDate = DateTime.Now;
        DateTime currentDate = GameObject.Find("WorldMenuManager").GetComponent<WorldMenuBehaviour>().getCurrentDate();
        if (DateTime.Equals(localDate.Date, currentDate.Date))
        {
            DataObject[] data = await dataAPI.simpleRequest();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data);
        }
        else
        {
            DataObject[][] currentData;
            if (worldMenu.GetComponent<WorldMenuBehaviour>().timeLapseIsOn)
            {
                DateTime start = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("start"));
                DateTime end = DateTime.Parse(worldMenu.GetComponent<WorldMenuBehaviour>().getDate("end"));
                currentData = await dataAPI.dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
                saveDataList.Add(currentData);
            }
            else
            {
                currentData = await dataAPI.dateRequest(currentDate.ToString("yyyy-MM-dd"), currentDate.ToString("yyyy-MM-dd"));
                activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
            }
        }
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        button.GetComponent<ScrollButtonButton>().setDeletable(true);
        Destroy(button.GetComponent<ItemDragHandler>());
        getApiList().Add(button.GetComponent<IDataAPI>());
        getBtnList().Add(button);
        loadingIcon.SetActive(false);
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
            Destroy(infoButton);
            infoButton = null;
        }
    }

    public async Task saveTimeSpanData(DateTime start, DateTime end)
    {
        activateVisualizer.deleteDrawings();
        foreach (GameObject button in getBtnList())
        {
            DataObject[][] currentData = await button.GetComponent<IDataAPI>().dateRequest(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
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
