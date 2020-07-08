using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

/*
controlls the API menu
*/
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
    public GameObject slider;

    private string currentKey;
    private Dictionary<string, List<IDataAPI>> apiList = new Dictionary<string, List<IDataAPI>>();
    private Dictionary<string, List<GameObject>> btnList = new Dictionary<string, List<GameObject>>();
    private ActivateVizualizer activateVisualizer;
    private Dictionary<string,DataObject[][]> saveDataList = new Dictionary<string,DataObject[][]>();
    private GameObject infoButton;

    private void Awake()
    {
        worldMenu.GetComponent<WorldMenuBehaviour>().AddSelectionChangeObserver(this);
        activateVisualizer = GetComponent<ActivateVizualizer>();
    }

    public void onChange(GameObject previous, GameObject selected)
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

/*
Adds a button to the right menu and activates the API
@btn the btn that is added
*/
    public async Task addButton(GameObject btn)
    {
        loadingIcon.SetActive(true);
        ScrollButtonButton buttonScript = btn.GetComponent<ScrollButtonButton>();
        IDataAPI dataAPI = btn.GetComponent<IDataAPI>();
        DateTime localDate = DateTime.Now;
        DateTime currentDate = GameObject.Find("WorldMenuManager").GetComponent<WorldMenuBehaviour>().getCurrentDate();
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        button.GetComponent<ScrollButtonButton>().setDeletable(true);
        Destroy(button.GetComponent<ItemDragHandler>());
        getApiList().Add(button.GetComponent<IDataAPI>());
        getBtnList().Add(button);
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
                currentData = await dataAPI.dateRequest(start.ToString("yyyy-MM-dd") + "T00:00:00Z", end.ToString("yyyy-MM-dd") + "T00:00:00Z" );
                saveDataList.Add(buttonScript.key, currentData);
                float index = slider.GetComponent<Slider>().value;
                visualizeTimespanData((int) index);
            }
            else
            {
                currentData = await dataAPI.dateRequest(currentDate.ToString("yyyy-MM-dd") + "T00:00:00", currentDate.ToString("yyyy-MM-dd") + "T23:59:59");
                activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
            }
        }
        loadingIcon.SetActive(false);
    }

/*
    visualizes the data for a single day on the selected globe
    @date requested date
*/
    public async Task drawSingleDay(DateTime date) {
        loadingIcon.SetActive(true);
        foreach (GameObject button in getBtnList())
        {
            activateVisualizer.deleteDrawings();
            ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
            DataObject[][] currentData;
            if(DateTime.Equals(DateTime.Now.Date, date.Date))
            {
                 currentData = await button.GetComponent<IDataAPI>().dateRequest(date.ToString("yyyy-MM-dd") + "T00:00:00", DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ"));
            } else {
                 currentData = await button.GetComponent<IDataAPI>().dateRequest(date.ToString("yyyy-MM-dd") + "T00:00:00", date.ToString("yyyy-MM-dd") + "T23:59:59");
            }
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
            
        }
        loadingIcon.SetActive(false);
    }

/*
deletes a button from the menu.
*/
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
            if(saveDataList.Remove(infoButton.GetComponent<ScrollButtonButton>().key)) {
                Debug.Log("Element entfernt");
            }
            Destroy(infoButton);
            infoButton = null;
        }
    }

/*
    saves Data from a Timespan into a List.
    @param start Start-date for Timelaps
    @param end End-date for Timelaps
*/
    public async Task saveTimeSpanData(DateTime start, DateTime end)
    {
        loadingIcon.SetActive(true);
        saveDataList.Clear();
        activateVisualizer.deleteDrawings();
        foreach (GameObject button in getBtnList())
        {
            DataObject[][] currentData;
            if (DateTime.Equals(end.Date, DateTime.UtcNow.Date))
            {
             currentData = await button.GetComponent<IDataAPI>().dateRequest(start.Date.ToString("yyyy-MM-dd") + "T00:00:00Z", DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ"));
                
            } else {
            currentData = await button.GetComponent<IDataAPI>().dateRequest(start.Date.ToString("yyyy-MM-dd") + "T00:00:00Z", end.Date.ToString("yyyy-MM-dd") + "T23:59:59Z");

            }
            saveDataList.Add(button.GetComponent<ScrollButtonButton>().key, currentData);
            ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
            activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, currentData, 0);
        }
        loadingIcon.SetActive(false);
    }

/*
Visualizes the data from thr list for a specific index
@param index represents the date
*/
    private void visualizeTimespanData(float index)
    {
        activateVisualizer.deleteDrawings();
        GameObject[] buttons = getBtnList().ToArray();
        int counter = 0;
        foreach (DataObject[][] data in saveDataList.Values)
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
