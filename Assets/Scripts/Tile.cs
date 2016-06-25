using System;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour, IEquatable<Tile>, IComparable {

    public int X {
        get { return xCoor; }
    }
    private int xCoor;

    public int Y {
        get { return yCoor; }
    }
    private int yCoor;

    public bool IsPossibleTurn {
        get { return _isPossibleTurn; }
        set {
			_isPossibleTurn = value ? OnTrue() : OnFalse();
        }
    }
    private bool _isPossibleTurn;
    public const float Dimension = 1.0f; // since tile 1x1 in editor?

    [SerializeField] private ParticleSystem _particleSystem;

    private void Awake() {
        xCoor = (int)transform.position.x;
        yCoor = (int)transform.position.y;
        Messenger.AddListener("NextTurn", HidePossibleTurn);
        Messenger.AddListener("WallPicked", HidePossibleTurn);
    }

    private void HidePossibleTurn() {
		if (IsPossibleTurn != false) IsPossibleTurn = false;
    }

    private bool OnTrue() {
        _particleSystem.Play();
        return true;
    }

    private bool OnFalse() {
        _particleSystem.Stop();
        return false;
    }

    private void OnMouseDown() {
        if (WallsManager.PickedUpWall != null) {
            WallsManager.PickedUpWall.Picked = false;
        } else if (IsPossibleTurn) {
            Messenger<Vector3>.Broadcast("Move", transform.position);
        }        
    }

    private void OnMouseExit() {
        if (WallsManager.PickedUpWall != null) {
            WallsManager.PickedUpWall.OnTileExit();
        }
    }
    
    public bool Equals(Tile other) {
        if (other == null) return false;
        return (xCoor == other.xCoor) && (yCoor == other.yCoor);
    }

    public override bool Equals(object o) {
        if (o == null) return false;
        if (this.GetType() != o.GetType()) return false;
        Tile comparable = (Tile)o;
        return Equals(comparable);
    }

    public static bool operator == (Tile tile1, Tile tile2) {
      if (((object)tile1) == null || ((object)tile2) == null)
            return object.Equals(tile1, tile2);
      return tile1.Equals(tile2);
   }

   public static bool operator != (Tile tile1, Tile tile2) {
      if (((object)tile1) == null || ((object)tile2) == null)
            return ! object.Equals(tile1, tile2);
      return ! (tile1.Equals(tile2));
   }

    public override int GetHashCode() {
        return xCoor ^ yCoor;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder("[");
        sb.Append(xCoor).Append(",").Append(yCoor).Append("]");
        return sb.ToString();
    }

	public int CompareTo(object other) {
		if (other == null)
			return 1;
		Tile otherTile = other as Tile;
		if (otherTile != null) {
			if (this.X != otherTile.X)
				return this.X - otherTile.X;
			else
				return this.Y - otherTile.Y;
		} else
			throw new ArgumentException ("Object isn't Tile");
	}
}
