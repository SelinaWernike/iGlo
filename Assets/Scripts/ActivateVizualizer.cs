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

    public void deleteDrawings()
    {
        visualizer.ClearDrawings();
    }
    public void visualize(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor, DataObject[][] timeDatas, int index)
    {
        if (timeDatas != null)
        {
            visualizer.PrepareVisualization(key, method, curve, startColor, endColor);
            for (int i = 0; i < timeDatas.Length; i++)
            {
                if (timeDatas[i] != null)
                {

                    if (timeDatas[i][index] != null)
                    {
                        visualize(key, method, curve, startColor, endColor, timeDatas[i][index]);
                    }
                }
            }
            visualizer.FinishVisualization();
        } else {
            Debug.Log("Keine Werte für das Datum gefunden");
        }
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

    public void visualize(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor, DataObject obj)
    {
        visualizer.Visualize(key, obj.getLatitude(), obj.getLongitude(), obj.getValue());
    }
}
