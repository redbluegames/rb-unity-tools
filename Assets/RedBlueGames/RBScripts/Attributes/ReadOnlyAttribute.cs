using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Attribute that makes a Unity field uneditable (Read Only)
/// in the editor.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyAttribute"/> class.
    /// </summary>
    public ReadOnlyAttribute()
    {
    }
}