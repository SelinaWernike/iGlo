using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataAPI
{
    DataObject[] simpleRequest();
    DataObject[] specificRequest(string location);

    DataObject[] specificRequest(string location, string startDate, string endDate);
}
