using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Graph<T> {
    private int _vertices = 0;
    private int _edges;
    private Dictionary<T, List<T>> allVertices = new Dictionary<T, List<T>>();

    public int NumberOfVertices() {
        return _vertices;
    }

    public int NumberOEdges() {
        return _edges;
    }

    public void AddVertice(T newVertice) {
        if (!allVertices.ContainsKey(newVertice)) {
            allVertices.Add(newVertice, new List<T>());
            _vertices++;       
        }
    }

    public void AddEdge(T vertice1, T vertice2) {
        GetAdjacentVertices(vertice1).Add(vertice2);
        GetAdjacentVertices(vertice2).Add(vertice1);
        _edges++;
    }

    public List<T> GetAdjacentVertices(T vertice) {
        List<T> adjacentVertices = null;
        allVertices.TryGetValue(vertice, out adjacentVertices);
        return adjacentVertices;
    }

    public bool DeleteVertice(T vertice) {
        if (!allVertices.ContainsKey(vertice))
            return false;
        var adjVert = GetAdjacentVertices(vertice);
        foreach (var v in adjVert) {
            GetAdjacentVertices(v).Remove(vertice);
            _edges--;
        }
        _vertices--;
        return allVertices.Remove(vertice);
    }

    public List<T> DeleteVerticeAndStoreAdjVertices(T vertice) {
        if (!allVertices.ContainsKey(vertice)) return null;
        var adjVert = GetAdjacentVertices(vertice);
        foreach (var v in adjVert) {
            GetAdjacentVertices(v).Remove(vertice);
            _edges--;
        }
        allVertices.Remove(vertice);
        _vertices--;
        return adjVert;
    }

    public List<T> GetAllVertices() {
        return new List<T>(this.allVertices.Keys);
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Number of Vertices: ").Append(_vertices).Append("\nNumber of Edges: ").Append(_edges);  
        var keys = allVertices.Keys;
        foreach (var key in keys) {
            sb.Append(key + " Adjacent Vertices: ");
            var adjV = GetAdjacentVertices(key);
            foreach (T v in adjV) {
                sb.Append(" " + v + ",");
            }
        }
        return sb.ToString();
    }
}

