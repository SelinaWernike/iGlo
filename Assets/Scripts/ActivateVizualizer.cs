using System.Collections.Generic;
using UnityEngine;

public class ActivateVizualizer : MonoBehaviour
{
    public GameObject earth;
    private VisualizeDataScript visualizer;
    private List<DataObject[]> toVisualize;
    private int counter = 0;
    private void Start()
    {
        visualizer = earth.GetComponent<VisualizeDataScript>();
    }
    public void visualize()
    {
        List<IDataAPI> dataApi = GetComponent<ScrollButtonControl>().getApiList();
        foreach (IDataAPI api in dataApi)
        {
            toVisualize.Add(api.simpleRequest());
        }

        foreach (DataObject[] obj in toVisualize)
        {
            visualize(obj);
        }
    }

    public void visualize(DataObject[] obj)
    {
        foreach (DataObject dataObj in obj)
        {
            Debug.Log(counter + ": " + dataObj.ToString());
            visualizer.Visualize(dataObj.getLatitude(), dataObj.getLongitude(), dataObj.getValue());
            counter++;
        }
        visualizer.FinishVisualization();
    }
}
