using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCampaign()
    {
        Debug.Log("Play Campaign");
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Characters()
    {
        Debug.Log("Characters");
        audioSource.Play();
        SceneManager.LoadScene("Characters");
    }

    public void Gacha()
    {
        Debug.Log("Gacha");
        audioSource.Play();
        SceneManager.LoadScene("Gacha");
    }

    public void Events()
    {
        Debug.Log("Events");
        SceneManager.LoadScene("Events");
    }

    //exit the game
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
