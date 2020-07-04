using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        worldMenu.GetComponent<WorldMenuBehaviour>().AddSelectionChangeObserver(this);
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
        GameObject button = Instantiate(btn);
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        Destroy(button.GetComponent<ItemDragHandler>());
        getApiList().Add(button.GetComponent<IDataAPI>());
        getBtnList().Add(button);
        ScrollButtonButton buttonData = button.GetComponent<ScrollButtonButton>();
        ActivateVizualizer activateVisualizer = GetComponent<ActivateVizualizer>();
        DataObject[] data = button.GetComponent<IDataAPI>().simpleRequest();
        activateVisualizer.visualize(buttonData.key, buttonData.method, buttonData.interpolationCurve, buttonData.startColor, buttonData.endColor, data);
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
