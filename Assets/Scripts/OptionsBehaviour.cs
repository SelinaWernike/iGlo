using UnityEngine;
using UnityEngine.UI;

public class OptionsBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject earth;
    [SerializeField]
    private GameObject methodDropdown;
    [SerializeField]
    private GameObject startColorPicker;
    [SerializeField]
    private GameObject endColorPicker;

    private VisualizeDataScript visualizer;
    private FlexibleColorPicker startColorScript;
    private FlexibleColorPicker endColorScript;
    private Dropdown methodDropdownScript;
    private string currentKey;
    private Color currentEndColor;

    private void Start()
    {
        visualizer = earth.GetComponent<VisualizeDataScript>();
        startColorScript = startColorPicker.GetComponent<FlexibleColorPicker>();
        endColorScript = endColorPicker.GetComponent<FlexibleColorPicker>();
        methodDropdownScript = methodDropdown.GetComponent<Dropdown>();
    }

    public void OnApiButtonClick(ScrollButtonButton apiButton)
    {
        currentKey = apiButton.key;
        startColorScript.color = apiButton.startColor;
        // we can not set the color on a disabled script, so just store it until enabled
        if (endColorPicker.activeInHierarchy)
        {
            endColorScript.color = apiButton.endColor;
        }
        else
        {
            currentEndColor = apiButton.endColor;
        }
        methodDropdownScript.value = (int)apiButton.method;
    }

    public void OnStartColorSubmit()
    {
        foreach (ScrollButtonButton obj in Resources.FindObjectsOfTypeAll(typeof(ScrollButtonButton)))
        {
            if (obj.key == currentKey)
            {
                obj.startColor = startColorScript.color;
            }
        }
        visualizer.SetStartColor(currentKey, startColorScript.color);
    }

    public void OnEndColorSubmit()
    {
        foreach (ScrollButtonButton obj in Resources.FindObjectsOfTypeAll(typeof(ScrollButtonButton)))
        {
            if (obj.key == currentKey)
            {
                obj.endColor = endColorScript.color;
            }
        }
        visualizer.SetEndColor(currentKey, endColorScript.color);
    }

    public void OnVisualizationSelection(int value)
    {
        VisualizationMethod method = (VisualizationMethod)value;
        foreach (ScrollButtonButton obj in Resources.FindObjectsOfTypeAll(typeof(ScrollButtonButton)))
        {
            if (obj.key == currentKey)
            {
                obj.method = method;
            }
        }
        if (method == VisualizationMethod.COLORS)
        {
            bool wasInactive = endColorPicker.activeInHierarchy;
            endColorPicker.SetActive(true);
            if (wasInactive)
            {
                endColorScript.color = currentEndColor;
            }
        }
        else
        {
            endColorPicker.SetActive(false);
        }
        visualizer.SetVisualizationMethod(currentKey, method);
    }
}
