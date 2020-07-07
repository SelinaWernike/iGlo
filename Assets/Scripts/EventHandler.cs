using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EventHandler : MonoBehaviour
{
 /*
 manages the text-Values for the Slider
 */  
 public void onValueChange(float value) {
     Text newText =  GameObject.Find("CurrentDate").GetComponent<Text>();
        newText.text = "Tag" + value;
    }

}
