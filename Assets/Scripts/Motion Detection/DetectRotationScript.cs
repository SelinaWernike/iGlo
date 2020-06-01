using UnityEngine;
using Leap.Unity;

public class DetectRotationScript : MonoBehaviour
{
    [SerializeField]
    private float cutoffThreshold;
    [SerializeField]
    private int smoothBufferSize;
    [SerializeField]
    private DoubleFloatEvent onRotation;
    private HandModel handModel;
    private CircularBuffer<Vector2> smoothBuffer;
    private Quaternion lastRotation;
    private bool valid;

    private void OnValidate()
    {
        smoothBufferSize = Mathf.Max(1, smoothBufferSize);
    }

    private void Start()
    {
        handModel = GetComponentInParent<HandModel>();
        smoothBuffer = new CircularBuffer<Vector2>(smoothBufferSize);        
    }

    private void FixedUpdate()
    {
        if (valid)
        {
            Quaternion current = handModel.GetPalmRotation();
            Quaternion difference = current * Quaternion.Inverse(lastRotation);
            float xDifference = Mathf.DeltaAngle(difference.eulerAngles.y, 360);
            float yDifference = -Mathf.DeltaAngle(difference.eulerAngles.x, 360);
            smoothBuffer.Add(new Vector2(xDifference, yDifference));
            Vector2 meanDifference = applyCutoff(getMeanDifference());
            if (meanDifference.x != 0 || meanDifference.y != 0)
            {
                onRotation.Invoke(meanDifference.x, meanDifference.y);
            }
            lastRotation = current;
        }
    }

    private Vector2 applyCutoff(Vector2 current)
    {
        if (Mathf.Abs(current.x) < cutoffThreshold)
        {
            current.x = 0;
        }
        if (Mathf.Abs(current.y) < cutoffThreshold)
        {
            current.y = 0;
        }
        return current;
    }

    private Vector2 getMeanDifference()
    {
        Vector2 meanDifference = new Vector2();
        foreach (Vector2 difference in smoothBuffer)
        {
            meanDifference += difference;
        }
        return meanDifference / smoothBuffer.Size();
    }

    public void OnActivate()
    {
        lastRotation = handModel.GetPalmRotation();
        valid = true;
    }

    public void OnDeactivate()
    {
        valid = false;
    }
}
