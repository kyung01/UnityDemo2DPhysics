using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class KTimer
{
    float time, timeInit;
    public KTimer(float timeMax)
    {
        time = 0;
        timeInit = timeMax;
    }
    public bool tick(float timeElapsed)
    {
        time -= timeElapsed;
        return time <= 0;
    }
    public void rewind()
    {
        time = timeInit;
    }
}