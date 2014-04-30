using UnityEngine;
using System.Collections;

public class InputDevice
{
    string deviceName;

    public string DeviceName {
        get {
            return deviceName;
        }
    }

    public InputDevice (string name)
    {
        deviceName = name;
    }

}
