using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	
	public GameObject MainMenu;
	public GameObject optionsMenu;
	public GameObject aboutMenu;
	public Text TitleText;

	private string mainTitle;

	void Awake()
	{
		mainTitle = TitleText.text;
		ShowMenu("mainMenu");
	}

	public void ShowMenu(string name)
	{
		MainMenu.SetActive (false);
        aboutMenu.GetComponent<Button>().interactable = false;
		optionsMenu.GetComponent<Button>().interactable = false;

        switch (name) {
		case "mainMenu":
			MainMenu.SetActive (true);
			TitleText.text = mainTitle;
			break;
		case "optionsMenu":
			//optionsMenu.SetActive(true);
			TitleText.text = "Options";
			break;
		case "About":
			//aboutMenu.SetActive(true);
			TitleText.text = "About";
			break;
		}
	}

	public void LoadLevel(string leveltoLoad)
	{
		SceneManager.LoadScene(leveltoLoad);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
