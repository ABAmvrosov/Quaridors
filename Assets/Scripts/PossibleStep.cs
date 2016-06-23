using UnityEngine;

public class PossibleStep : MonoBehaviour {

    public bool Active {
        get { return _active; }
        set {
            _active = value ? Show() : Hide();            
        }
    }
    private bool _active = false;
    private SpriteRenderer _sprite;
    
    private bool Show() {
        _sprite.enabled = true;
        return true;
    }

    private bool Hide() {
        _sprite.enabled = false;
        return false;
    }

    private void OnWallPick() {
        Debug.Log("In OnWallPick");
        Active = false;
    }

    private void Awake() {
        Messenger.AddListener("WallPicked", OnWallPick);
        _sprite = this.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        Messenger<Vector3>.Broadcast("Move", transform.localPosition);
        Messenger.Broadcast("NextTurn");
    }
}
