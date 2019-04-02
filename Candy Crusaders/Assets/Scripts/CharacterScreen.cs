using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScreen : MonoBehaviour
{
    private void Update()
    {
        //Go back to main menu
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Assets/InteractiveMobileMenu/Scenes/Menu.unity");
        }
    }
}
