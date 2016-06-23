using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paths<T> {

    private bool[] marked;
    private int[] edgeTo;
    private List<T> TList;
    private T origin;

    public Paths(Graph<T> graph, T origin) {
        TList = graph.GetAllVertices();
        marked = new bool[graph.NumberOfVertices()];
        edgeTo = new int[graph.NumberOfVertices()];
        this.origin = origin;
        FindAllPaths(graph, origin);
    }

    private void FindAllPaths(Graph<T> graph, T origin) {
        int tNumber = TList.IndexOf(origin);
        marked[tNumber] = true;
        foreach (int adjacentTNumber in NumbersOfAdjacentT(graph, origin)) {
            if (!marked[adjacentTNumber]) {
                edgeTo[adjacentTNumber] = tNumber;
                FindAllPaths(graph, TList[adjacentTNumber]);
            }
        }
    }

    private List<int> NumbersOfAdjacentT(Graph<T> graph, T origin) {
        List<int> result = new List<int>();
        foreach (T vertice in graph.GetAdjacentVertices(origin)) {
            result.Add(TList.IndexOf(vertice));
        }
        return result;
    }

    public bool hasPathTo(T destination) {
        return marked[TList.IndexOf(destination)];
    }
}
