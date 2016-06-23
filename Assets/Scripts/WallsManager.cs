using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallsManager : MonoBehaviour {
    public static Wall PickedUpWall { get; set; }

    public LayerMask WallLayerMask {
        get { return _layerMask; }
    }
    private LayerMask _layerMask;            

    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int _numberOfWalls;
    [SerializeField] private Text _wallCounter;
    private Stack<GameObject> _wallPool;

    private void Awake() {
        _layerMask = LayerMask.GetMask("Walls");
         _wallPool = new Stack<GameObject>(_numberOfWalls);
        GameObject wallObj;
        for (int i = 0; i < _numberOfWalls; i++) {
            wallObj = Instantiate(_wallPrefab) as GameObject;
            wallObj.SetActive(false);
            wallObj.transform.SetParent(this.transform);
            _wallPool.Push(wallObj);
        }
    }

    public void SpawnWall() {
        if (_numberOfWalls > 1) {
            _wallPool.Pop().SetActive(true);
            _numberOfWalls--;
            _wallCounter.text = "Walls Left: " + _numberOfWalls;
        } else {
            _numberOfWalls--;
            _wallCounter.text = "Walls Left: " + _numberOfWalls;
        }
    }
}
