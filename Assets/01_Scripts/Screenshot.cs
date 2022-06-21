using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    private Camera screenshotCam;

    public Sprite ScreenshotToSprite()
    {
        int width = Screen.width;
        int height = Screen.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);

        screenshotCam.targetTexture = renderTexture;

        screenshotCam.Render();
        RenderTexture.active = renderTexture;

        Texture2D screenShot = new Texture2D(width, width, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, (height - width) * 0.5f, width, width), 0, 0);
        screenShot.Apply();

        screenshotCam.targetTexture = null;

        Rect rect = new Rect(0, 0, screenShot.width, screenShot.height);
        Sprite sprite = Sprite.Create(screenShot, rect, Vector2.one * 0.5f);

        return sprite;
    }
}
