using UnityEngine;
using UnityEngine.Events;
using Leap.Unity;

public class DetectMovementInDirectionScript : MonoBehaviour
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private UnityEvent onActivate;
    private HandModel handModel;
    private Vector3 startPoint;
    private bool valid;

    private void OnValidate()
    {
        direction = direction.normalized;
    }

    private void Start()
    {
        handModel = GetComponentInParent<HandModel>();
    }

    private void FixedUpdate()
    {
        if (valid)
        {
            if (Vector3.Dot(direction, handModel.GetPalmPosition()) - Vector3.Dot(direction, startPoint) > distance)
            {
                onActivate.Invoke();
                valid = false;
            }
        }
    }

    public void ChangeDirection()
    {
        direction = -direction;
        OnActivate();
    }

    public void OnActivate()
    {
        startPoint = handModel.GetPalmPosition();
        valid = true;
    }

    public void OnDeactivate()
    {
        valid = false;
    }
}
