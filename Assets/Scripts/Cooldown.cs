using System.Collections;
using UnityEngine;

public class Cooldown
{
    float startWaitTime = 0;
    float duration = 0;

    // Constructor
    public Cooldown(float duration)
    {
        this.duration = duration;
    }

    // Functions

    public bool Wait()
    {
        float nextAvailableTime = startWaitTime + duration;
        if (Time.time > nextAvailableTime)
        {
            startWaitTime = Time.time;
            return true;
        }
        return false;
    }

    public void Refresh()
    {
        startWaitTime = Time.time;
    }
}
