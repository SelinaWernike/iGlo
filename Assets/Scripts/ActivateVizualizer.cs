using System.Collections.Generic;
using UnityEngine;

public class ActivateVizualizer : MonoBehaviour
{
    public GameObject earth;
    private VisualizeDataScript visualizer;
    private int counter = 0;
    private void Start()
    {
        visualizer = earth.GetComponent<VisualizeDataScript>();
    }

    public void visualize(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor, DataObject[] obj)
    {
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
