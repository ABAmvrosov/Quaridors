using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class TilesManager : MonoBehaviour {
    
    private Graph<Tile> _graph;
    private Dictionary<string, Tile> _tilesByName;
    [SerializeField] private Transform[] _playersTransform;
	[SerializeField] private GameObject[] _winTileContainers;
	private List<Tile[]> _winTiles;

	private void Awake() {
		_tilesByName = new Dictionary<string, Tile>();
		InitGraph();
		//Debug.Log(_graph.ToString());
	}

	private void InitGraph() {
		var tiles = GetComponentsInChildren<Tile>();
		_graph = new Graph<Tile>();
		foreach (var tile in tiles) {
			tile.name = tile.ToString(); // for editor comfort
			_tilesByName.Add(tile.name, tile);
			_graph.AddVertice(tile);
			ProceedDirection(tile, Vector2.down);
			ProceedDirection(tile, Vector2.right);
		}
	}

	private void ProceedDirection(Tile tile, Vector2 direction) {
		int tmp = tile.gameObject.layer;
		tile.gameObject.layer = 2;
		RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(tile.X, tile.Y), direction, Tile.Dimension);
		foreach (var hit in hits) {
			if (hit.collider != null && hit.collider.CompareTag("SimpleGround")) {
				Tile dstTile = hit.collider.GetComponent<Tile>();
				_graph.AddVertice(dstTile);
				_graph.AddEdge(tile, dstTile);
			}
		}        
		tile.gameObject.layer = tmp;
	}    

    public List<Tile> AdjacentTiles(Tile origin) {
        return _graph.GetAdjacentVertices(origin);
    }

    public List<Tile> AdjacentTiles(Vector3 coordinates) {
        return AdjacentTiles(GetTile(coordinates));
    }

    public bool IsAbleToPathIfWall(params Transform[] blockers) {
        Tile[] origins = new Tile[blockers.Length];
        List<Tile>[] deletedEdges = new List<Tile>[blockers.Length];
        int counter = 0;
        foreach (var blocker in blockers) {
            origins[counter] = GetTile(blocker.position);
            deletedEdges[counter] = _graph.DeleteVerticeAndStoreAdjVertices(origins[counter]);
            counter++;
        }
		bool[] canPlayersPath = new bool[_playersTransform.Length];
		for (int i = 0; i < _playersTransform.Length - 1; i++) {
			CanPlayerPath(i, ref canPlayersPath[i]);
		}
        deletedEdges[1].Remove(origins[0]);
        deletedEdges[0].Remove(origins[1]);
        for (int i = 0; i < origins.Length; i++) {
            _graph.AddVertice(origins[i]);
            foreach (Tile tile in deletedEdges[i]) {
                _graph.AddEdge(origins[i], tile);
            }
        }
        _graph.AddEdge(origins[0], origins[1]);
		return IsFalseInBoolArray(canPlayersPath);
    }

	private bool CanPlayerPath(int playerNumber, ref bool canPlayerPath) {
		Paths<Tile> paths = new Paths<Tile>(_graph, GetTile(_playersTransform[playerNumber].position));
		foreach (Tile tile in _winTiles[playerNumber]) {
			if (paths.hasPathTo(tile)) {
				canPlayerPath = true;
				return canPlayerPath;
			}
		}
		return false;
	}

	private bool IsFalseInBoolArray(bool[] array) {
		bool result = true;
		foreach (bool b in array) {
			result = result & b;
		}
		return !result;
	}

    public Tile GetTile(Vector3 coordinates) {
        //Debug.Log("1. Coordinates before cast: " + coordinates.x + ", " + coordinates.y);
        int x = coordinates.x > 0 ? (int)(coordinates.x + 0.1f) : (int)(coordinates.x - 0.1f);
        int y = coordinates.y > 0 ? (int)(coordinates.y + 0.1f) : (int)(coordinates.y - 0.1f);
        //Debug.Log("2. Coordinates after cast: " + x + ", " + y);
        StringBuilder sb = new StringBuilder();
        sb.Append("[").Append(x).Append(",").Append(y).Append("]");
        Tile result;
        _tilesByName.TryGetValue(sb.ToString(), out result);
        return result;
    }

    public void RemoveTile(Vector3 coordinates) {
        Tile tile = GetTile(coordinates);
        tile.gameObject.SetActive(false);
        _graph.DeleteVertice(GetTile(coordinates));
    }
}
