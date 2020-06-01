using UnityEngine;

public class TransformEarthScript : MonoBehaviour
{
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float minScale;

    private void OnMouseDrag()
    {
        Rotate(Input.GetAxis("Mouse X") * 3, Input.GetAxis("Mouse Y") * 3);
    }

    private void Update()
    {
        Scale(Input.mouseScrollDelta.y * 0.001f);
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
