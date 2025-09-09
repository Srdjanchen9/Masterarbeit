
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    

    [Header("Main Menu Buttons")]
    public Button startButton;
   
    public Button quitButton;
    public Button backtoMainMenuButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        //EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        //optionButton.onClick.AddListener(EnableOption);
        //aboutButton.onClick.AddListener(EnableAbout);
        quitButton.onClick.AddListener(QuitGame);
        //backtoMainMenuButton.onClick.AddListener(BackToMainMenu);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StroopTest");
    }
    
}
