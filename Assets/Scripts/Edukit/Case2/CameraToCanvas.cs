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
        // Render Texture 설정
        if (secondaryCamera != null && renderTexture != null)
        {
            secondaryCamera.targetTexture = renderTexture;
        }

        // Raw Image 설정
        if (rawImage != null && renderTexture != null)
        {
            rawImage.texture = renderTexture;
        }
    }
}
