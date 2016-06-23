using System;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour, IEquatable<Tile> {

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
            _isPossibleTurn = value ? ShowPossibleTurn() : HidePossibleTurn();
        }
    }
    private bool _isPossibleTurn;
    public const float Dimension = 1.0f; // since tile 1x1 in editor?

    [SerializeField] private ParticleSystem _particleSystem;

    private void Awake() {
        xCoor = (int)Math.Floor(transform.position.x);
        yCoor = (int)Math.Floor(transform.position.y);
        Messenger.AddListener("NextTurn", Hide);
        Messenger.AddListener("WallPicked", Hide);
    }

    private void Hide() {
        IsPossibleTurn = false;
    }

    private bool ShowPossibleTurn() {
        _particleSystem.Play();
        return true;
    }

    private bool HidePossibleTurn() {
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
}
