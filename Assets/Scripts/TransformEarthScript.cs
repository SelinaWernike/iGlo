using UnityEngine;
using UnityEngine.EventSystems;

public class TransformEarthScript : MonoBehaviour, IScrollHandler, IDragHandler
{
    [SerializeField]
    private float dragSensitivity;
    [SerializeField]
    private float scrollSensitivity;
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float minScale;

    public void OnDrag(PointerEventData eventData)
    {
        Rotate(eventData.delta.x * dragSensitivity, eventData.delta.y * dragSensitivity);
    }

    public void OnScroll(PointerEventData eventData)
    {
        Scale(eventData.scrollDelta.y * scrollSensitivity);
    }

    public void Rotate(float xDifference, float yDifference)
    {
        if (xDifference != 0)
        {
            transform.Rotate(Vector3.down, xDifference, Space.World);
        }
        if (yDifference != 0)
        {
            transform.Rotate(Vector3.right, yDifference, Space.World);
        }
    }

    public void Scale(float difference)
    {
        if (difference != 0)
        {
            transform.localScale += new Vector3(difference, difference, difference);
            transform.localScale = Vector3.Max(transform.localScale, new Vector3(minScale, minScale, minScale));
            transform.localScale = Vector3.Min(transform.localScale, new Vector3(maxScale, maxScale, maxScale));
        }
    }
}
