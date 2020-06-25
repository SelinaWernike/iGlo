using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum VisualizationMethod
{
    SATURATION,
    RADIUS
}

public class VisualizeDataScript : MonoBehaviour
{
    private const int EARTH_RADIUS = 6378;

    [SerializeField]
    private int radius;
    [SerializeField]
    private int numCirclePoints;
    [SerializeField]
    private Color color;
    [SerializeField]
    private VisualizationMethod method;
    private Texture2D texture;
    private Color[] original;
    private List<Record> values;

    private void Start()
    {
        values = new List<Record>();
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
    }

    public void Visualize(float latitude, float longitude, float data, bool lastData)
    {
        values.Add(new Record(latitude, longitude, data));
        Debug.Log("call: " + latitude + ", " + longitude);
        if (lastData)
        {
            float min = values.Min(v => v.data);
            float max = values.Max(v => v.data);
            foreach (Record record in values)
            {
                Visualize(record.latitude, record.longitude, record.data, min, max);
            }
            values.Clear();
        }
    }

    public void Visualize(float latitude, float longitude, float data, float min, float max)
    {
        switch (method)
        {
            case VisualizationMethod.SATURATION:
                {
                    float h, s, v;
                    Color.RGBToHSV(color, out h, out s, out v);
                    float newSaturation = Map(data, min, max, 0, 1);
                    DrawPoint(latitude, longitude, radius, Color.HSVToRGB(h, newSaturation, v));
                    break;
                }
            case VisualizationMethod.RADIUS:
                {
                    float newRadius = Map(data, min, max, radius / 4, radius);
                    DrawPoint(latitude, longitude, newRadius, color);
                    break;
                }
        }
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
        texture.Apply(true);
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

    private struct Record
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
