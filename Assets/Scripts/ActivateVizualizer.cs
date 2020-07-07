using UnityEngine;
using System;

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
            if (timeDatas.Length != 0)
            {

                visualizer.PrepareVisualization(key, method, curve, startColor, endColor);
                for (int i = 0; i < timeDatas.Length; i++)
                {
                    if (timeDatas[i] != null)
                    {
                        if (timeDatas[i].Length != 0)
                        {

                            if (timeDatas[i][index] != null)
                            {
                                int trueIndex = index;
                                try
                                {
                                    trueIndex = checkDate(timeDatas, i, index);
                                }
                                catch (ArgumentNullException e)
                                {
                                    Debug.Log(e);
                                    continue;
                                }
                                DataObject obj = timeDatas[i][trueIndex];
                                Debug.Log(obj);
                                visualizer.Visualize(key, obj.getLatitude(), obj.getLongitude(), obj.getValue());
                            }
                        }
                    }
                }
            }
            visualizer.FinishVisualization();
        }
        else
        {
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

    public void deleteVisual(string key)
    {
        GameObject earth = worldMenu.GetComponent<WorldMenuBehaviour>().GetSelectedEarth();
        VisualizeDataScript visualizer = earth.GetComponent<VisualizeDataScript>();
        visualizer.ClearByKey(key);
    }

    /**
        Compares the Current Date with the Date from the DataObject to map the correct value to the globe
    **/
    private int checkDate(DataObject[][] data, int index1, int index2)
    {
        if (data[index1][index2] == null || data[index1].Length < index2 + 1)
        {
            throw new ArgumentNullException("Datum nicht in Array");
        }
        Debug.Log(data[index1][index2].getDate() + " , " + worldMenu.GetComponent<WorldMenuBehaviour>().getCurrentDate());
        if (DateTime.Equals(data[index1][index2].getDate().Date, worldMenu.GetComponent<WorldMenuBehaviour>().getCurrentDate().Date))
        {
            Debug.Log("true");
            return index2;
        }
        return checkDate(data, index1, index2 + 1);
    }
}
