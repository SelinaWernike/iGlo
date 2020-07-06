using UnityEngine;

public class ActivateVizualizer : MonoBehaviour
{
    public GameObject worldMenu;

    public void deleteDrawings()
    {
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        visualizer.ClearDrawings();
    }

    public void visualize(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor, DataObject[][] timeDatas, int index)
    {
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        if (timeDatas != null)
        {
            visualizer.PrepareVisualization(key, method, curve, startColor, endColor);
            for (int i = 0; i < timeDatas.Length; i++)
            {
                if (timeDatas[i] != null)
                {
                    if (timeDatas[i][index] != null)
                    {
                        DataObject obj = timeDatas[i][index];
                        visualizer.Visualize(key, obj.getLatitude(), obj.getLongitude(), obj.getValue());
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
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        visualizer.PrepareVisualization(key, method, curve, startColor, endColor);
        foreach (DataObject dataObj in obj)
        {
            visualizer.Visualize(key, dataObj.getLatitude(), dataObj.getLongitude(), dataObj.getValue());
        }
        visualizer.FinishVisualization();
    }

    public void deleteVisual(string key) {
         GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        visualizer.ClearByKey(key);
    }
}
