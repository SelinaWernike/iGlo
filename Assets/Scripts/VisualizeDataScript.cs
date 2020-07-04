using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum VisualizationMethod
{
    RADIUS,
    SATURATION,
    COLORS
}

public class VisualizeDataScript : MonoBehaviour
{
    private const int EARTH_RADIUS = 6378;

    [SerializeField]
    private int radius;
    [SerializeField]
    private int numCirclePoints;

    private Texture2D texture;
    private Color[] original;
    private Dictionary<string, Visualiuation> values;

    private void Start()
    {
        values = new Dictionary<string, Visualiuation>();
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        original = texture.GetPixels();
    }

    private void OnApplicationQuit()
    {
        ClearDrawings();
    }

    public void ClearDrawings()
    {
        texture.SetPixels(original);
        texture.Apply(true);
        values.Clear();
    }

    public void Redraw()
    {
        if (values.Count > 0)
        {
            texture.SetPixels(original);
            FinishVisualization();
        }
    }

    public void SetStartColor(string key, Color color)
    {
        Visualiuation visualiuation;
        if (values.TryGetValue(key, out visualiuation) && visualiuation.startColor != color)
        {
            visualiuation.startColor = color;
            Redraw();
        }
    }

    public Color GetStartColor(string key)
    {
        return values[key].startColor;
    }

    public void SetEndColor(string key, Color color)
    {
        Visualiuation visualiuation;
        if (values.TryGetValue(key, out visualiuation) && visualiuation.endColor != color)
        {
            visualiuation.endColor = color;
            Redraw();
        }
    }

    public Color GetEndColor(string key)
    {
        return values[key].endColor;
    }

    public void SetVisualizationMethod(string key, VisualizationMethod method)
    {
        Visualiuation visualiuation;
        if (values.TryGetValue(key, out visualiuation) && visualiuation.method != method)
        {
            visualiuation.method = method;
            Redraw();
        }
    }

    public VisualizationMethod GetVisualization(string key)
    {
        return values[key].method;
    }

    public void PrepareVisualization(string key, VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor)
    {
        values[key] = new Visualiuation(method, curve, startColor, endColor);
    }

    public void Visualize(string key, float latitude, float longitude, float data)
    {
        values[key].records.Add(new Record(latitude, longitude, data));
    }

    public void FinishVisualization()
    {
        foreach (Visualiuation visualiuation in values.Values)
        {
            float min = visualiuation.records.Min(v => v.data);
            float max = visualiuation.records.Max(v => v.data);
            foreach (Record record in visualiuation.records)
            {
                switch (visualiuation.method)
                {
                    case VisualizationMethod.SATURATION:
                        {
                            float h, s, v;
                            Color.RGBToHSV(visualiuation.startColor, out h, out s, out v);
                            float newSaturation = Interpolate(record.data, min, max, 0, 1, visualiuation.curve);
                            DrawPoint(record.latitude, record.longitude, radius, Color.HSVToRGB(h, newSaturation, v));
                            break;
                        }
                    case VisualizationMethod.RADIUS:
                        {
                            float newRadius = Interpolate(record.data, min, max, radius / 4, radius, visualiuation.curve);
                            DrawPoint(record.latitude, record.longitude, newRadius, visualiuation.startColor);
                            break;
                        }
                    case VisualizationMethod.COLORS:
                        {
                            float proportion = Interpolate(record.data, min, max, 0, 1, visualiuation.curve);
                            DrawPoint(record.latitude, record.longitude, radius, Color.Lerp(visualiuation.startColor, visualiuation.endColor, proportion));
                            break;
                        }
                }
            }
        }
        texture.Apply(true);
    }

    private void DrawPoint(float latitude, float longitude, float radius, Color color)
    {
        Vector2Int[] points = new Vector2Int[numCirclePoints];
        Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);
        for (int i = 0; i < numCirclePoints; i++)
        {
            float angle = ((360f / numCirclePoints) * i);
            // get the latitude and longitude of a point with a given angle and distance
            Vector2 point = GetLatLongFromDistance(latitude, longitude, radius, angle);
            // since we use a equirectangular projection, we can directly map them to x/y coordinates based on the textures size
            int x = Mathf.RoundToInt(Map(point.y, -180, 180, 0, texture.width));
            int y = Mathf.RoundToInt(Map(point.x, -90, 90, 0, texture.height));
            points[i] = new Vector2Int(x, y);
            // store min and maximum point of the ellipsoid so iterating over pixels is faster
            min = new Vector2Int(Mathf.Min(x, min.x), Mathf.Min(y, min.y));
            max = new Vector2Int(Mathf.Max(x, max.x), Mathf.Max(y, max.y));
        }
        // iterate over the pixels in the texture and set them to the color if they are inside the ellipsoid
        for (int i = min.x; i <= max.x; i++)
        {
            for (int j = min.y; j <= max.y; j++)
            {
                if (PointInside(i, j, points))
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }
    }

    private bool PointInside(int x, int y, Vector2Int[] points)
    {
        bool result = false;
        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            if ((points[i].y > y) != (points[j].y > y) && (x < (points[j].x - points[i].x) * (y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
            {
                result = !result;
            }
        }
        return result;
    }

    private float Interpolate(float value, float from1, float to1, float from2, float to2, AnimationCurve curve)
    {
        float percentage = (value - from1) / (to1 - from1);
        percentage = curve.Evaluate(percentage);
        return (to2 - from2) * percentage + from2;
    }

    private float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private Vector2 GetLatLongFromDistance(float latStart, float longStart, float distance, float angle)
    {
        latStart = latStart * Mathf.Deg2Rad;
        longStart = longStart * Mathf.Deg2Rad;
        angle = angle * Mathf.Deg2Rad;
        float lat = Mathf.Asin(Mathf.Sin(latStart) * Mathf.Cos(distance / EARTH_RADIUS) + Mathf.Cos(latStart) * Mathf.Sin(distance / EARTH_RADIUS) * Mathf.Cos(angle));
        float lon = longStart + Mathf.Atan2(Mathf.Sin(angle) * Mathf.Sin(distance / EARTH_RADIUS) * Mathf.Cos(lat), Mathf.Cos(distance / EARTH_RADIUS) - Mathf.Sin(lat) * Mathf.Sin(lat));
        return new Vector2(lat * Mathf.Rad2Deg, lon * Mathf.Rad2Deg);
    }

    private class Visualiuation
    {
        public VisualizationMethod method;
        public AnimationCurve curve;
        public Color startColor, endColor;
        public List<Record> records;

        public Visualiuation(VisualizationMethod method, AnimationCurve curve, Color startColor, Color endColor)
        {
            this.method = method;
            this.curve = curve;
            this.startColor = startColor;
            this.endColor = endColor;
            this.records = new List<Record>();
        }
    }

    private class Record
    {
        public float latitude, longitude, data;

        public Record(float latitude, float longitude, float data)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.data = data;
        }
    }
}
