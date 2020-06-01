using UnityEngine;
using Leap.Unity;
using System.Threading;

public class DetectFingerDistanceScript : MonoBehaviour
{
    [SerializeField]
    private GameObject hand1;
    [SerializeField]
    private GameObject hand2;
    [SerializeField]
    private int fingerIndex;
    [SerializeField]
    private FloatEvent onMovement;
    private FingerModel finger1;
    private FingerModel finger2;
    private int activeFingers;
    private float previousDistance;
    private bool valid;
    private object mutex = new object();

    private void OnValidate()
    {
        fingerIndex = Mathf.Min(Mathf.Max(fingerIndex, 0), HandModel.NUM_FINGERS - 1);
    }

    private void Start()
    {
        finger1 = hand1.GetComponent<HandModel>().fingers[fingerIndex];
        finger2 = hand2.GetComponent<HandModel>().fingers[fingerIndex];
    }

    private void FixedUpdate()
    {
        if (valid)
        {
            float distance = Vector3.Distance(finger1.GetTipPosition(), finger2.GetTipPosition());
            float change = (distance - previousDistance) / 75;
            if (change != 0)
            {
                onMovement.Invoke(change);
            }
            previousDistance = distance;
        }
    }

    public void OnActivate()
    {
        if (++activeFingers == 2)
        {
            previousDistance = Vector3.Distance(finger1.GetTipPosition(), finger2.GetTipPosition());
            valid = true;
        }
    }

    public void OnDeactivate()
    {
        --activeFingers;
        valid = false;
    }
}
