using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainScenario : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI texthighScore;

    private void Awake()
    {
        texthighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void BtnClickGameStart()
    {
        SceneManager.LoadScene("02Game");
    }

    public void BtnClickGameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit;
#endif
    }
}
