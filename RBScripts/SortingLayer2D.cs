using UnityEngine;
using System.Collections;

public class SortingLayer2D : MonoBehaviour {

	public string sortLayer;

	void Start () {
		renderer.sortingLayerName = sortLayer;
	}
}
