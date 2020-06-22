using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonButton : MonoBehaviour
{
    [SerializeField]
    private Text btnText;
    public void SetText(string text) {
        btnText.text = text;

    }

    public void onClick() {

    }
}
