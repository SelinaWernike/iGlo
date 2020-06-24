using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DeleteBtnEvent : MonoBehaviour
{
    public GameObject dataMenu;
    public GameObject addMenu;
    public void deleteAll() {
        List<GameObject> deleteList = dataMenu.GetComponent<ScrollButtonControl>().getBtnList();
        deleteList.ForEach(btn => Destroy(btn));
        dataMenu.GetComponent<ScrollButtonControl>().setBtnList(new List<GameObject>());
        dataMenu.GetComponent<ScrollButtonControl>().setApiList(new List<IDataAPI>());

        foreach(Transform child in addMenu.transform) {
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
