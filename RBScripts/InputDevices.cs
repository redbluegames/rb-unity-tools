using UnityEngine;
using System.Collections;

public static class InputDevices
{
    //TODO We need a data structure that can be accessed by name, iterated on, and retricted to
    // only certain keys.
    public enum ControllerTypes
    {
        Keyboard = 0,
        XBox
    }

    static InputDevice[] inputDevices = {new InputDevice ("PC"), new InputDevice ("XBox")};

    static public InputDevice[] GetAllInputDevices()
    {
        return inputDevices;
    }
}