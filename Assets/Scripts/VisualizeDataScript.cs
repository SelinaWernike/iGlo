using UnityEngine;

public class VisualizeDataScript : MonoBehaviour
{
    private const int EARTH_RADIUS = 6371;

    [SerializeField]
    private int radius;
    [SerializeField]
    private int numCirclePoints;
    [SerializeField]
    private Color color;
    private Texture2D texture;
    private Color[] original;

    private void Start()
    {
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        original = texture.GetPixels();
    }

    public void ClearDrawings()
    {
        texture.SetPixels(original);
        texture.Apply(false);
    }

    public void DrawPoints(float latitude, float longitude, int amount)
    {
        
    }

    public void DrawPoint(float latitude, float longitude)
    {
        Vector2Int[] points = new Vector2Int[numCirclePoints];
        Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);
        for (int i = 0; i < numCirclePoints; i++)
        {
            float angle = ((360f / numCirclePoints) * i);
            Vector2 point = GetLatLongFromDistance(latitude, longitude, radius, angle);
            int x = Mathf.RoundToInt(Map(point.y, -180, 180, 0, texture.width));
            int y = Mathf.RoundToInt(Map(point.x, -90, 90, 0, texture.height));
            points[i] = new Vector2Int(x, y);
            min = new Vector2Int(Mathf.Min(x, min.x), Mathf.Min(y, min.y));
            max = new Vector2Int(Mathf.Max(x, max.x), Mathf.Max(y, max.y));
        }
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
        texture.Apply(false);
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
}
