using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ScrollButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    private List<IDataAPI> apiList = new List<IDataAPI>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void buttonSpawn(int i) {
        GameObject button = Instantiate(buttonTemplate) as GameObject;
        button.SetActive(true);

        button.GetComponent<ScrollButtonButton>().SetText("Button: " + i);
        button.transform.SetParent(buttonTemplate.transform.parent,false);

    }

    public void addButton(GameObject btn) {
        GameObject button = Instantiate(btn) as GameObject;
        button.SetActive(true);
        button.transform.SetParent(buttonTemplate.transform.parent,false);
        Destroy(button.GetComponent<ItemDragHandler>());
        apiList.Add(button.GetComponent<IDataAPI>());
        int i = 0;
    }
}
