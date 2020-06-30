using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonButton : MonoBehaviour
{
    public GameObject menuInfo;
    private Text btnText;

    public void SetText(string text) {
        btnText.text = text;

    }

    public void onClick() {
        menuInfo.SetActive(true);
        GameObject.Find("InfoTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getDescription();
        GameObject.Find("TitleTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getName();


    }
}
