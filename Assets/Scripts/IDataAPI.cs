using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataAPI<T,V>
{
    T simpleRequest();
    V specificRequest(string location);

    V specificRequest(string location, string startDate, string endDate);
}
