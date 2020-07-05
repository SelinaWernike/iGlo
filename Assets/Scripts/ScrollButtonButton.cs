using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonButton : MonoBehaviour
{
    public GameObject menuInfo;
    public GameObject scrollMenu;
    public AnimationCurve interpolationCurve;
    public VisualizationMethod method;
    public GameObject deleteBtn;
    public Color startColor;
    public Color endColor;
    private bool deletable = false;

    public string key;

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


    public void setDeletable(bool deletable)
    {
        this.deletable = deletable;
    }
}
