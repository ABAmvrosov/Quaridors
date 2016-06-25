using UnityEngine;
using UnityEditor;
using System;

public class MenuItems {

	[MenuItem("Tools/Rename tiles -> [IntX, IntY]")]
	private static void RenameObjects() {
		GameObject container = GameObject.FindGameObjectWithTag ("TilesManager");
		Tile[] allTiles = container.GetComponentsInChildren<Tile> ();
		foreach (Tile tile in allTiles) {
			tile.name = "[" + (int)tile.transform.position.x + "," + (int)tile.transform.position.y + "]";		
		}
	}
}
