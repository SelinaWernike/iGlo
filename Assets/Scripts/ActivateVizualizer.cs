using UnityEngine;

public class ActivateVizualizer : MonoBehaviour
{
    public GameObject worldMenu;
    private int counter = 0;

    public void visualize(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor, DataObject[] obj)
    {
        counter = 0;
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        visualizer.PrepareVisualization(key, method, curve, startColor, endColor);
        foreach (DataObject dataObj in obj)
        {
            Debug.Log(counter + ": " + dataObj.ToString());
            visualizer.Visualize(key, dataObj.getLatitude(), dataObj.getLongitude(), dataObj.getValue());
            counter++;
        }
        visualizer.FinishVisualization();
    }
}
