using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayCampaign()
    {
        Debug.Log("Play Campaign");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Characters()
    {
        Debug.Log("Characters");
        SceneManager.LoadScene("Characters");
    }

    public void Gacha()
    {
        Debug.Log("Gacha");
        SceneManager.LoadScene("Gacha");
    }


    //exit the game
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
