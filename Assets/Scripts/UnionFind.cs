using System.Collections;
using System;

public class UnionFind {
	private int[] _parent;
	private int[] _size;
	private int _count;

	public UnionFind(int N) {
		_count = N;
		_parent = new int[N];
		_size = new int[N];
		for (int i = 0; i < N; i++) {
			_parent[i] = i;
			_size[i] = 1;
		}
	}

	public int Count() {
		return _count;
	}

	public int Find(int p) {
		Validate (p);
		while (p != _parent[p])
			p = _parent[p];
		return p;
	}

	private void Validate(int p) {
		int n = _parent.Length;
		if (p < 0 || p >= n) {
			throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + (n - 1));
		}
	}

	public Boolean Connected(int p, int q) {
		return Find(p) == Find(q);
	}

	public void Union(int p, int q) {
		int rootP = Find(p);
		int rootQ = Find(q);
		if (rootP == rootQ)
			return;
		if (_size[rootP] < _size[rootQ])
		{
			_parent[rootP] = rootQ;
			_size[rootQ] += _size[rootP];
		}
		else {
			_parent[rootQ] = rootP;
			_size[rootP] += _size[rootQ];
		}
		_count--;
	}
}









