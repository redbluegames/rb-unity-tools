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
using System.Collections;

namespace RedBlueTools
{
	public class RBdfFloatingLabel : MonoBehaviour
	{
		public Camera mainCamera;
		public dfLabel label;
		dfControl myControl;
		dfGUIManager manager;
		string labelText;

		public string LabelText {
			get {
				return labelText;
			}
			set {
				labelText = value;
				label.Text = labelText;
			}
		}

		Color labelColor;

		public Color LabelColor {
			get {
				return labelColor;
			}
			set {
				labelColor = value;
				label.Color = labelColor;
			}
		}
	
		Vector3 worldPosition;

		public Vector3 WorldPosition {
			get {
				return worldPosition;
			}
			set {
				worldPosition = value;
				Vector2 screenPoint = manager.ScreenToGui (mainCamera.WorldToScreenPoint (worldPosition));
				myControl.RelativePosition = (Vector3)screenPoint;
			}
		}

		void Awake ()
		{	
			if (mainCamera == null) {
				mainCamera = Camera.main;
				if (mainCamera == null) {
					Debug.LogError ("RBdfFloatingLabel component is unable to determine which camera is the MainCamera", gameObject);
				}
			}

			myControl = GetComponent<dfControl> ();
			if (myControl == null) {
				Debug.LogError ("RBdfFloatingLabel not set up: " + gameObject.name, gameObject);
				this.enabled = false;
			}
			manager = myControl.GetManager ();

			if (label == null) {
				label = GetComponentInChildren<dfLabel> ();

				if (label == null) {
					Debug.LogError ("RBdfFloatingLabel not set up. Must add a label as a child game object or specify one.");
				}
			}
		}
	}
}
