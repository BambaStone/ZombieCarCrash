using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject MenuUI;
    private bool _gameStop = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameStop)
            {
                MenuOff();
            }
            else
            {
                MenuOn();
            }
        }
    }
    private void FixedUpdate()
    {
        
    }

    void MenuOff()
    {
        MenuUI.SetActive(false);
        _gameStop = false;
        Time.timeScale = 1f;
    }
    void MenuOn()
    {
        MenuUI.SetActive(true);
        _gameStop = true;
        Time.timeScale = 0f;
    }

    public void SaveButton()
    {
        StatusManager.Instance.SaveGame();
    }
    public void MainButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
    public void QuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
