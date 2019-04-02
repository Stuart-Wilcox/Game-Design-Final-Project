using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    public Player player;

    public Transform characterSelect;

    public Transform content;

    private List<Toggle> characterSelects;

    public void Start()
    {
        this.characterSelects = new List<Toggle>();

        int offset = 100;
        foreach (Character character in this.player.characters)
        {
            this.CreateCharacterSelect(character, offset);
            offset -= 40;
        }
        
    }

    public void CreateCharacterSelect(Character character, int offset)
    {
        Transform transform = Instantiate(this.characterSelect, new Vector3(0, offset, 0), Quaternion.Euler(0,0,0), this.content);
        Toggle characterSelect = transform.GetComponent<Toggle>();

        characterSelect.transform.GetChild(1).GetComponent<Text>().text = character.name;
        characterSelect.isOn = character.isActive;
        this.characterSelects.Add(characterSelect);
    }

    public List<bool> GetWhichSelected()
    {
        List<bool> whichSelected = new List<bool>();
        foreach(Toggle characterSelect in this.characterSelects)
        {
            whichSelected.Add(characterSelect.isOn);
        }

        return whichSelected;
    }

    public void ContinueToLevel()
    {
        List<bool> whichSelected = this.GetWhichSelected();

        List<Character> activeCharacters = new List<Character>();
        for(int i = 0; i < whichSelected.Count; i++)
        {
            if (whichSelected[i])
            {
                activeCharacters.Add(player.characters[i]);
            }
        }

        player.activeCharacters = activeCharacters.ToArray();

        // load the scene
        string sceneName = "Assets/Scenes/Level" + player.activeLevel + ".unity";
        SceneManager.LoadScene(sceneName);
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene(1);
    }
}
