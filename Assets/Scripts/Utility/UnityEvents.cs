using System;
using UnityEngine.Events;

[Serializable]
public class DoubleFloatEvent : UnityEvent<float, float> { }

[Serializable]
public class IntEvent : UnityEvent<int> { }

[Serializable]
public class FloatEvent : UnityEvent<float> { }

