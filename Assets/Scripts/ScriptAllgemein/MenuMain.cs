
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    //public GameObject options;
    //public GameObject about;

    [Header("Main Menu Buttons")]
    public Button startButton;
    //public Button optionButton;
    //public Button aboutButton;
    public Button quitButton;
    public Button backtoMainMenuButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        //optionButton.onClick.AddListener(EnableOption);
        //aboutButton.onClick.AddListener(EnableAbout);
        quitButton.onClick.AddListener(QuitGame);
        backtoMainMenuButton.onClick.AddListener(BackToMainMenu);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneManager.LoadScene("2 StroopTest");
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        //options.SetActive(false);
        //about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        //options.SetActive(false);
        //about.SetActive(false);
    }
    public void EnableOption()
    {
        mainMenu.SetActive(false);
        //options.SetActive(true);
        //about.SetActive(false);
    }
    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        //options.SetActive(false);
        //about.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StroopTest");
    }
}
