using UnityEngine;

public class OptionsBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject earth;
    [SerializeField]
    private GameObject startColorPicker;
    [SerializeField]
    private GameObject endColorPicker;
    private VisualizeDataScript visualizer;
    private FlexibleColorPicker startColorScript;
    private FlexibleColorPicker endColorScript;

    private void Start()
    {
        visualizer = earth.GetComponent<VisualizeDataScript>();
        startColorScript = startColorPicker.GetComponent<FlexibleColorPicker>();
        endColorScript = endColorPicker.GetComponent<FlexibleColorPicker>();
    }

    public void OnStartColorSubmit()
    {
        visualizer.SetStartColor(startColorScript.color);
    }

    public void OnEndColorSubmit()
    {
        visualizer.SetEndColor(endColorScript.color);
    }

    public void OnVisualizationSelection(int value)
    {
        VisualizationMethod method = (VisualizationMethod)value;
        endColorPicker.SetActive(method == VisualizationMethod.COLORS);
        visualizer.SetVisualizationMethod(method);
    }
}
