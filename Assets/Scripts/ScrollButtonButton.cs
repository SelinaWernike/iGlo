using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonButton : MonoBehaviour
{
    public GameObject menuInfo;
    public AnimationCurve interpolationCurve;
    public VisualizationMethod method;
    public Color startColor;
    public Color endColor;

    public string key;

    public void onClick()
    {
        menuInfo.SetActive(true);
        GameObject.Find("InfoTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getDescription();
        GameObject.Find("TitleTxt").GetComponent<Text>().text = GetComponent<IDataAPI>().getName();
        GetComponentInParent<OptionsBehaviour>().OnApiButtonClick(this);
    }
}
