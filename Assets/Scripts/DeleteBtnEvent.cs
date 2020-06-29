using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteBtnEvent : MonoBehaviour
{
    public GameObject dataMenu;
    public GameObject addMenu;
    public GameObject earth;
  
    public void deleteAll()
    {
        List<GameObject> deleteList = dataMenu.GetComponent<ScrollButtonControl>().getBtnList();
        deleteList.ForEach(btn => Destroy(btn));
        dataMenu.GetComponent<ScrollButtonControl>().setBtnList(new List<GameObject>());
        dataMenu.GetComponent<ScrollButtonControl>().setApiList(new List<IDataAPI>());

        foreach (Transform child in addMenu.transform)
        {
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        earth.GetComponent<VisualizeDataScript>().ClearDrawings();
    }

    
   public void onValueChange(float value) {
        GetComponent<Text>().text = "Tag" + value;
    }
}