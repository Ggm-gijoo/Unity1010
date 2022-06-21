using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [Header("기본")]
    [SerializeField]
    private StageController stageController;

    [Space(10f)]

    [Header("게임 내")]
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;
    [SerializeField]
    private TextMeshProUGUI textHighScore;

    [Space(10f)]

    [Header("게임 오버")]
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private Screenshot screenshot;
    [SerializeField]
    private Image imageScreenshot;
    [SerializeField]
    private TextMeshProUGUI textResultScore;

    private void Update()
    {
        textCurrentScore.text = stageController.CurrentScore.ToString();
        textHighScore.text = stageController.HighScore.ToString();
    }

    public void BtnClickHome()
    {
        SceneManager.LoadScene("01Main");
    }
    public void BtnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        imageScreenshot.sprite = screenshot.ScreenshotToSprite();
        textResultScore.text = stageController.CurrentScore.ToString();

        panelGameOver.SetActive(true);
    }

}
