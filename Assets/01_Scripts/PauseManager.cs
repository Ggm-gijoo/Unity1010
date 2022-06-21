using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject imageBackGroundOverlay;
    [SerializeField]
    private Animator panelAnim;

    public void OnAppear()
    {
        imageBackGroundOverlay.SetActive(true);
        gameObject.SetActive(true);
        panelAnim.Play("OnPanel");
    }

    public void OnDisappear()
    {
        panelAnim.Play("OffPanel");
    }

    public void EndOfDisappear()
    {
        imageBackGroundOverlay.SetActive(false);
        gameObject.SetActive(false);
    }
}
