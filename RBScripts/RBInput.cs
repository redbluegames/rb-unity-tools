using UnityEngine;
using System.Collections;

public class RBInput {

    const string PLAYER_PREFIX = "_P";
    const string DEVICE_PREFIX = "_";

    public static bool GetButtonDownForPlayer(string buttonName, int playerIndex, InputDevice device)
    {
//        return Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, device));
		bool xbox = Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
		bool keyboard = Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
		return xbox || keyboard;
    }
	
	public static bool GetButtonUpForPlayer(string buttonName, int playerIndex, InputDevice device)
    {
//        return Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, device));
		bool xbox = Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
		bool keyboard = Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
		return xbox || keyboard;
    }

    public static bool GetButtonForPlayer(string buttonName, int playerIndex, InputDevice device)
    {
//        return Input.GetButton (ConcatPlayerIndex (buttonName, playerIndex, device));
		bool xbox = Input.GetButton (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
		bool keyboard = Input.GetButton (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
		return xbox || keyboard;
    }

    public static float GetAxisRawForPlayer (string axisName, int playerIndex, InputDevice device)
    {
//        return Input.GetAxisRaw (ConcatPlayerIndex(axisName, playerIndex, device));
		float xbox = Input.GetAxisRaw (ConcatPlayerIndex (axisName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
		float keyboard = Input.GetAxisRaw (ConcatPlayerIndex (axisName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
		return xbox + keyboard;
    }

    public static float GetAxisForPlayer (string axisName, int playerIndex, InputDevice device)
    {
//        return Input.GetAxis (ConcatPlayerIndex (axisName, playerIndex, device));
		float xbox = Input.GetAxis (ConcatPlayerIndex (axisName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
		float keyboard = Input.GetAxis (ConcatPlayerIndex (axisName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
		return xbox + keyboard;
	}

    static string ConcatPlayerIndex (string buttonName, int playerIndex, InputDevice device)
    {
        return buttonName + DEVICE_PREFIX + device.DeviceName + PLAYER_PREFIX + playerIndex.ToString ();
    }
}
