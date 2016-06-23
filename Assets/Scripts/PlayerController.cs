using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float _moveSpeed = 2.0f;
    private Animator _animator;

    private bool IsPlayerTurn {
        get { return GameManager.Instance.ActivePlayer == id;  }
    }
    [SerializeField] private Player id = Player.one;
    private bool IsMoving {
        get { return _isMoving; }
        set {
            _isMoving = value;
            if (value) {
                _animator.SetBool("isMoving", true);
            } else {
                _animator.SetBool("isMoving", false);
            }
        }
    }
    private bool _isMoving;

    private void Awake() {
        ShowPossibleSteps();
        _animator = GetComponent<Animator>();
        Messenger.AddListener("NextTurn", ShowPossibleSteps);
        Messenger.AddListener("Win", OnWin);
        Messenger<Vector3>.AddListener("Move", Move);
    }

    private void Update() {
    }

    private void ShowPossibleSteps() {
        if (IsPlayerTurn) {
            var adjacentTiles = GameManager.Instance.TilesManager.AdjacentTiles(transform.position);
            foreach (Tile tile in adjacentTiles) {
                //Debug.Log(tile);
                tile.IsPossibleTurn = true;
            }
        } 
    }

    private void Move(Vector3 destination) {
        if (IsPlayerTurn && !IsMoving) {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, destination - transform.position, Mathf.Infinity, GameManager.WallsManager.WallLayerMask);
            //if (hit.collider != null)
            //    Debug.Log("There is wall!");
            StartCoroutine(SmoothMoving(destination + Vector3.forward));
        }
    }

    private IEnumerator SmoothMoving(Vector3 destination) {
        IsMoving = true;
        while (transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        Messenger.Broadcast("NextTurn");
        IsMoving = false;
    }

    private void OnWin() {
        if (IsPlayerTurn)
            _animator.SetBool("isWin", true);
    }
}
