using UnityEngine;
using UnityEngine.UI;

/*
methods and attributes for API-Buttons
*/
public class ScrollButtonButton : MonoBehaviour
{
    public GameObject menuInfo;
    public GameObject scrollMenu;
    public AnimationCurve interpolationCurve;
    /*
    method for how to visualize the button
    */
    public VisualizationMethod method;
    public GameObject deleteBtn;
    public Color startColor;
    public Color endColor;
    private bool deletable = false;

/*
key that identifies the API button
*/
    public string key;

/*
activates when button is clicked and displays information for the api
*/
    public void onClick()
    {
        menuInfo.SetActive(true);
        deleteBtn.SetActive(false);
        if (deletable)
        {
            deleteBtn.SetActive(true);
            scrollMenu.GetComponent<ScrollButtonControl>().setActiveBtn(this.gameObject);
        }
        GameObject.Find("InfoTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getDescription();
        GameObject.Find("TitleTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getName();
        GetComponentInParent<OptionsBehaviour>().OnApiButtonClick(this);
    }

/*
changes if the button can be deleted
*/
    public void setDeletable(bool deletable)
    {
        this.deletable = deletable;
    }
}
