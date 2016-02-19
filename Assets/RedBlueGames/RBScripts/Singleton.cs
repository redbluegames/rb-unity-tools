/*****************************************************************************
 *  Copyright (C) 2014-2015 Red Blue Games, LLC
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
 
/* Code grabbed from http://wiki.unity3d.com/index.php/Singleton
 * 
 * Be aware this will not prevent a non singleton constructor
 *   such as `T myT = new T();`
 * To prevent that, add `protected T () {}` to your singleton class.
 * As a note, this is made as MonoBehaviour because we need Coroutines.
 */ 
namespace RedBlue
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static object _lock = new object ();
 
		public static T Instance {
			get {
				if (applicationIsQuitting) {
					Debug.LogWarning ("[Singleton] Instance '" + typeof(T) +
						"' already destroyed on application quit." +
						" Won't create again - returning null.");
					return null;
				}
 
				lock (_lock) {
					if (_instance == null) {
						_instance = (T)FindObjectOfType (typeof(T));
 
						if (FindObjectsOfType (typeof(T)).Length > 1) {
							Debug.LogError ("[Singleton] Something went really wrong " +
								" - there should never be more than 1 singleton!" +
								" Reopenning the scene might fix it.");
							return _instance;
						}
 
						if (_instance == null) {
							GameObject singleton = new GameObject ();
							_instance = singleton.AddComponent<T> ();
							singleton.name = "(singleton) " + typeof(T).ToString ();
 
							DontDestroyOnLoad (singleton);
 
							Debug.Log ("[Singleton] An instance of " + typeof(T) + 
								" is needed in the scene, so '" + singleton +
								"' was created with DontDestroyOnLoad.");
						} else {
							Debug.Log ("[Singleton] Using instance already created: " +
								_instance.gameObject.name);
						}
					}
 
					return _instance;
				}
			}
		}
		
		static bool applicationIsQuitting = false;
		
		// WORKAROUND: Allow bool to be reflagged so that Unit Tests can set up singletons in editor.
		public static void ResetSingleton ()
		{
			applicationIsQuitting = false;
		}

		/*
		 * When Unity quits, it destroys objects in a random order.
		 * In principle, a Singleton is only destroyed when application quits.
		 * 
		 * If any script calls Instance after it have been destroyed, 
		 * it will create a buggy ghost object that will stay on the Editor scene
		 * even after stopping playing the Application. Really bad!
		 * So, this was made to be sure we're not creating that buggy ghost object.
		 */
		public virtual void OnApplicationQuit ()
		{
			applicationIsQuitting = true;
		}
	}
}