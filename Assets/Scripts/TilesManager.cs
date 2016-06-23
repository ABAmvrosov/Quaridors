using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class TilesManager : MonoBehaviour {
    
    private Graph<Tile> _graph;
    private Dictionary<string, Tile> _tilesByName;
    [SerializeField] private Transform[] _playersTransform;
    [SerializeField] private Tile[] _winTilePlayer1;
    [SerializeField] private Tile[] _winTilePlayer2;
    
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
        bool canPlayer1Path = false;
        bool canPlayer2Path = false;
        Paths<Tile> paths1 = new Paths<Tile>(_graph, GetTile(_playersTransform[0].position));
        Paths<Tile> paths2 = new Paths<Tile>(_graph, GetTile(_playersTransform[1].position));
        foreach (Tile tile in _winTilePlayer1) {
            if (paths1.hasPathTo(tile)) {
                canPlayer1Path = true;
                break;
            }                
        }
        foreach (Tile tile in _winTilePlayer2) {
            if (paths2.hasPathTo(tile)) {
                canPlayer2Path = true;
                break;
            }
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
        return canPlayer1Path & canPlayer2Path;
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

    public void TestFunc() {
        Paths<Tile> paths = new Paths<Tile>(_graph, GetTile(new Vector3(3.0f, -6.0f)));
        Debug.Log(paths.hasPathTo(GetTile(new Vector3(-1.0f, 4.0f))));
    }

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
}
