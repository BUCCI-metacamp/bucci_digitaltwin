using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraToCanvas : MonoBehaviour
{
    public Camera secondaryCamera;
    public RawImage rawImage;
    public RenderTexture renderTexture;

    void Start()
    {
        // Render Texture ����
        if (secondaryCamera != null && renderTexture != null)
        {
            secondaryCamera.targetTexture = renderTexture;
        }

        // Raw Image ����
        if (rawImage != null && renderTexture != null)
        {
            rawImage.texture = renderTexture;
        }
    }
}
