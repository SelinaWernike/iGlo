using UnityEngine;

public class LatLongScript : MonoBehaviour
{
    [SerializeField]
    private DoubleFloatEvent onRaycast;
    private bool valid;

    private void OnMouseDrag()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            valid = false;
        }
    }

    private void OnMouseDown()
    {
        valid = true;
    }

    private void OnMouseUp()
    {
        if (valid)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);
                    Vector2 latLon = ToLatLong(localPoint);
                    onRaycast.Invoke(latLon.x, latLon.y);
                }
            }
        }
    }

    private Vector2 ToLatLong(Vector3 position)
    {
        position = Vector3.Normalize(position);
        float lat = (float)Mathf.Asin(position.y) * Mathf.Rad2Deg;
        float lon = -(float)Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg;
        return new Vector2(lat, lon);
    }
}
