using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Image Save1;
    public Image Save2;
    public Image Save3;
    public GameObject SaveDatas;
    public GameObject CameraTurn;
    private bool Save1On = false;
    private bool Save2On = false;
    private bool Save3On = false;



    private void FixedUpdate()
    {
        CameraTurn.transform.Rotate(0,Time.deltaTime*10,0);
    }
    public void Save1Button()
    {
        if(Save1On)
        {
            Save1On = false;
            Save1.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            SaveDatas.SetActive(false);
        }
        else
        {
            SaveDatas.SetActive(false);
            StatusManager.Instance.SaveNum = 1;
            StatusManager.Instance.LoadGame();
            SaveDatas.SetActive(true);
            Save1On = true;
            Save2On = false;
            Save3On = false;
            Save1.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            Save2.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            Save3.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
        }
    }
    public void Save2Button()
    {
        if (Save2On)
        {
            Save2On = false;
            Save2.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            SaveDatas.SetActive(false);
        }
        else
        {
            SaveDatas.SetActive(false);
            StatusManager.Instance.SaveNum = 2;
            StatusManager.Instance.LoadGame();
            SaveDatas.SetActive(true);
            Save2On = true;
            Save1On = false;
            Save3On = false;
            Save2.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            Save1.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            Save3.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
        }
    }
    public void Save3Button()
    {
        if (Save3On)
        {
            Save3On = false;
            Save3.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            SaveDatas.SetActive(false);
        }
        else
        {
            SaveDatas.SetActive(false);
            StatusManager.Instance.SaveNum = 3;
            StatusManager.Instance.LoadGame();
            SaveDatas.SetActive(true);
            Save3On = true;
            Save2On = false;
            Save1On = false;
            Save3.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            Save2.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
            Save1.color = new Color(0.9f, 0.9f, 0.9f, 0.7f);
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ResetButton()
    {
        StatusManager.Instance.NewGame();
    }

    public void QuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
