using UnityEngine;

public class WinTile : Tile {
    public Player WinForPlayer {
        get { return _winCondition; }
    }
    [SerializeField] private Player _winCondition;
    private void OnMouseDown() {
        if (WallsManager.PickedUpWall != null) {
            WallsManager.PickedUpWall.Picked = false;
        } else if(IsPossibleTurn) {
            Messenger<Vector3>.Broadcast("Move", transform.position);
            if (GameManager.Instance.ActivePlayer == _winCondition) {
                Messenger.Broadcast("Win");
            }
            Messenger.Broadcast("NextTurn");
        }
    }
}
