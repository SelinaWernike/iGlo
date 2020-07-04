using System.Collections.Generic;
using UnityEngine;
public class DeleteBtnEvent : MonoBehaviour
{
    public GameObject dataMenu;
    public GameObject infoMenu;
    
    public GameObject addMenu;
    public GameObject worldMenu;
  
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
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        earth.GetComponent<VisualizeDataScript>().ClearDrawings();
    }

    
    public void closeBtnClick() {
        infoMenu.SetActive(false);
    }
}