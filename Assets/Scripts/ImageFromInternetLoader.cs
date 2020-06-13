using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Loads image from internet
/// </summary>
public class ImageFromInternetLoader : MonoBehaviour
{
    /// <summary>
    /// Url from where to load image
    /// </summary>
    public string url = "";

    /// <summary>
    /// Reload image from url after X seconds
    /// 0 means that no auto reload should be executed
    /// </summary>
    public float updateInterval = 1;

    /// <summary>
    /// Key for shared data property (if empty no shared data automatic setting will be used)
    /// </summary>
    public string sharedDataKey;

    /// <summary>
    /// Executes thias action on schene start
    /// </summary>
    void Start()
    {
        this.StartCoroutine(this.DownloadImage());
    }

    /// <summary>
    /// Used for the coroutine to download image
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DownloadImage()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(this.url);
            yield return request.SendWebRequest();

            Texture2D iTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Rect iTextRect = new Rect(0, 0, iTexture.width, iTexture.height);
            Vector2 iTextPivot = new Vector2(0.5f, 0.5f);

            Sprite tmpSprite = Sprite.Create(iTexture, iTextRect, iTextPivot);
            this.gameObject.GetComponent<Image>().sprite = tmpSprite;
            if (sharedDataKey.Length > 0)
            {
                typeof(SharedData).GetField(this.sharedDataKey, BindingFlags.Public | BindingFlags.Static).SetValue(null, tmpSprite);
            }

            if (this.updateInterval > 0)
            {
                yield return new WaitForSeconds(this.updateInterval);
            }
            else
            {
                break;
            }
        }
    }

}
