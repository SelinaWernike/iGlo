using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= 10; i++) {
            buttonSpawn(i);
        }
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
}
