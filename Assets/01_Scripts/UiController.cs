using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Update()
    {
        textCurrentScore.text = stageController.CurrentScore.ToString();
        textHighScore.text = stageController.HighScore.ToString();
    }

}
