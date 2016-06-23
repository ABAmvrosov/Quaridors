using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public bool Picked {
        get { return _picked; }
        set {
            _picked = value ? OnPickUp() : OnDrop();
        }
    }
    private bool _picked;
    private bool _overGround;
    private Collider2D _collider2D;
    private LayerMask _layerMask;
    private List<Transform> blockers;

    public void OnTileExit() {
        _overGround = false;
    }

    private void Awake() {
        _layerMask = LayerMask.GetMask("Player", "Walls", "Borders");
        _collider2D = this.GetComponent<Collider2D>();
        blockers = new List<Transform>();
        foreach (var childTransform in GetComponentsInChildren<Transform>()) {
            if (childTransform.tag == "Blocker") {
                blockers.Add(childTransform);
            }
        }
    }

    private bool OnPickUp() {
        Messenger.Broadcast("WallPicked");
        WallsManager.PickedUpWall = this;
        return true;
    }

    private bool OnDrop() {
        if (isGroundFree() && IsAbleToPathIfWall()) {
            RemoveTileUnderneath();
            WallsManager.PickedUpWall = null;
            GameManager.Instance.WallsManager.SpawnWall();
            Messenger.Broadcast("NextTurn");
            return false;
        } else return true;
    }

    private bool isGroundFree() {
        return !(_collider2D.IsTouchingLayers(_layerMask));
    }

    private bool IsAbleToPathIfWall() {
        return GameManager.Instance.TilesManager.IsAbleToPathIfWall(blockers.ToArray());
    }

    private void RemoveTileUnderneath() {
        var origins = GetComponentsInChildren<Transform>();
        foreach (var blocker in blockers) {
            GameManager.Instance.TilesManager.RemoveTile(blocker.position);
        }
    }

    public void TestFunc() {
        if (_collider2D.IsTouchingLayers(_layerMask)) {
        }
    }

    private void FixedUpdate() {
        if (Picked)
            MouseFollow();
        if (Input.GetButtonDown("Fire2"))
            Rotate();
    }

    private void Rotate() {
        if (Picked)
            transform.Rotate(Vector3.forward * 90f);
    }

    private void OnMouseDown() {
        Picked = true;
    }

    private void MouseFollow() {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("SimpleGround")) {
            ClipToGround(hit.collider.GetComponent<Tile>());
        } else {
            _overGround = false;
            point.z = gameObject.transform.position.z;
            gameObject.transform.position = point;
        }
    }

    private void ClipToGround(Tile tile) {
        if (!_overGround) {
            Vector3 newPosition = tile.transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            //Debug.Log("Target Position: " + tile.transform.position);
            _overGround = true;
        }
    }
}
