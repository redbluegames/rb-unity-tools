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
namespace RedBlueGames.Tools
{
    using UnityEngine;

    /// <summary>
    /// Class that draws a simple gizmo in the Editor
    /// </summary>
    public class Gizmo : MonoBehaviour
    {
        [Tooltip("The size for the gizmos")]
        [SerializeField]
        private float gizmoSize = 0.5f;

        [Tooltip("The color for the gizmos")]
        [SerializeField]

        private Color gizmoColor = Color.yellow;

        private void OnDrawGizmos()
        {
            Gizmos.color = this.gizmoColor;
            Gizmos.DrawWireSphere(transform.position, this.gizmoSize);

            // Draw X line
            float lineSize = this.gizmoSize * 2;
            Vector3 position = transform.position + transform.TransformDirection(Vector3.forward * lineSize);
            Gizmos.DrawLine(transform.position, position);

            // Draw Z line
            position = transform.position + transform.TransformDirection(Vector3.right * lineSize);
            Gizmos.DrawLine(transform.position, position);
        }
    }
}