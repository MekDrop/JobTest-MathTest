using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Image loader that loads image depending on state
/// </summary>
public class ImageDependingOnResultLoader : MonoBehaviour
{

    /// <summary>
    /// Sprite that is displayed when player has won
    /// </summary>
    public Sprite winSprite;

    /// <summary>
    /// Sprite is displayed when player has lost
    /// </summary>
    public Sprite lostSprite;

    /// <summary>
    /// Executes this function when schene starts
    /// </summary>
    void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = SharedData.hasWon ? winSprite : lostSprite;
    }

}
