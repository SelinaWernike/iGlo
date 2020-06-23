using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMenuEvent : MonoBehaviour
{
    public GameObject menu;
    public void close() {
        menu.SetActive(false);
    }

    public void open() {
        menu.SetActive(true);
    }
}
