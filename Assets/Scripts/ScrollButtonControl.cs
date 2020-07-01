using System.Collections.Generic;
using UnityEngine;

public class ScrollButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private List<IDataAPI> apiList = new List<IDataAPI>();
    private List<GameObject> btnList = new List<GameObject>();

    public void addButton(GameObject btn)
    {
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(parent.transform, false);
        button.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(button.GetComponent<ItemDragHandler>());
        apiList.Add(button.GetComponent<IDataAPI>());
        btnList.Add(button);
        ScrollButtonButton buttonScript = button.GetComponent<ScrollButtonButton>();
        ActivateVizualizer activateVisualizer = GetComponent<ActivateVizualizer>();
        DataObject[] data = button.GetComponent<IDataAPI>().simpleRequest();
        activateVisualizer.visualize(buttonScript.key, buttonScript.method, buttonScript.interpolationCurve, buttonScript.startColor, buttonScript.endColor, data);
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
