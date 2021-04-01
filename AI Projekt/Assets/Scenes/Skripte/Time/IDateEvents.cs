using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDateEvents
{
    void Event_DayUpdated(Date date);
    void Event_UpdateDate(Date date);
}
