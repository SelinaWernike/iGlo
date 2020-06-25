using UnityEngine;

public class OptionsBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject earth;
    private VisualizeDataScript visualizer;

    private void Start()
    {
        visualizer = earth.GetComponent<VisualizeDataScript>();
    }

    public void OnVisualizationSelection(int value)
    {
        visualizer.SetVisualizationMethod((VisualizationMethod)value);
    }
}
