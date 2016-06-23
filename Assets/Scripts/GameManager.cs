using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Player { one, two }

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public TilesManager TilesManager { get; set; }
    public WallsManager WallsManager { get; set; }
    public Player ActivePlayer {
        get { return _activePlayer; }
        private set { _activePlayer = value; }
    }
    [SerializeField] private Text _playerTurn;
    [SerializeField] private Player _activePlayer = Player.one;
    [SerializeField] private GameObject _winScreen; 

    private void Awake() {
        if (Instance == null)
            Instance = this;
        TilesManager = GameObject.FindGameObjectWithTag("TilesManager").GetComponent<TilesManager>();
        WallsManager = GameObject.FindGameObjectWithTag("WallsManager").GetComponent<WallsManager>();
        Messenger.AddListener("NextTurn", NextPlayer);
        Messenger.AddListener("Win", OnWin);
    }

    private void NextPlayer() {
        if (_activePlayer == Player.one) {
            _activePlayer = Player.two;
            _playerTurn.text = "Player 2 Turn!";
        } else {
            _activePlayer = Player.one;
            _playerTurn.text = "Player 1 Turn!";
        }
    }

    private void OnWin() {
        _winScreen.SetActive(true);
    }

    public void LoadLevel(string leveltoLoad) {
        SceneManager.LoadScene(leveltoLoad);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
