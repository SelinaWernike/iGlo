using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDetailsForPointScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool valid;

    public void OnDrag(PointerEventData eventData)
    {
        valid = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        valid = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (valid)
        {
            RaycastResult result = eventData.pointerPressRaycast;
            Vector3 localPoint = result.gameObject.transform.InverseTransformPoint(result.worldPosition);
            Vector2 latLon = ToLatLong(localPoint);
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
