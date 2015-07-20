/*****************************************************************************
 *  Red Blue Tools are Unity Editor utilities. Some utilities require 3rd party tools.
 *  Copyright (C) 2014 Red Blue Games, LLC
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ****************************************************************************/
using UnityEngine;

namespace RedBlueTools
{
	public class RBInput
	{

		const string PLAYER_PREFIX = "_P";
		const string DEVICE_PREFIX = "_";

		public static bool GetButtonDownForPlayer (string buttonName, int playerIndex, InputDevice device)
		{
//        return Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, device));
			bool xbox = Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
			bool keyboard = Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
			return xbox || keyboard;
		}
	
		public static bool GetButtonUpForPlayer (string buttonName, int playerIndex, InputDevice device)
		{
//        return Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, device));
			bool xbox = Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.XBox]));
			bool keyboard = Input.GetButtonUp (ConcatPlayerIndex (buttonName, playerIndex, InputDevices.GetAllInputDevices () [(int)InputDevices.ControllerTypes.Keyboard]));
			return xbox || keyboard;
		}

		public static bool GetButtonForPlayer (string buttonName, int playerIndex, InputDevice device)
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
}