using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    int scene;

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Music1");
        this.scene = 1;
    }

    public void PreLoadGame1()
    {
        this.scene = 1;
        Invoke(nameof(this.SelectPlayGame), 0.1f);
    }
    public void PreLoadGame2()
    {
        this.scene = 2;
        Invoke(nameof(this.SelectPlayGame), 0.1f);
    }

    public void SelectPlayGame()
    {
        //SceneManager.LoadScene("Game");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene(scene);
    }
    public void HoverButton()
    {
        FindObjectOfType<AudioManager>().Play("Hover");
    }
    public void SelectButton()
    {
        FindObjectOfType<AudioManager>().Play("Select");
    }

}
