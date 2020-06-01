using UnityEngine;
using Leap;
using Leap.Unity;

public class CountFingersScript : MonoBehaviour
{
    [SerializeField]
    private IntEvent onFingerCountChange;
    private HandModel handModel;
    private int fingerCount;

    private void Start()
    {
        handModel = GetComponentInParent<HandModel>();
    }

    private void FixedUpdate()
    {
        int extendenCount = 0;
        foreach (FingerModel finger in handModel.fingers)
        {
            Finger leapFinger = finger.GetLeapFinger();
            if (leapFinger.IsExtended)
            {
                extendenCount++;
            }
        }
        if (extendenCount != fingerCount)
        {
            fingerCount = extendenCount;
            onFingerCountChange.Invoke(fingerCount);
        }
    }
}
