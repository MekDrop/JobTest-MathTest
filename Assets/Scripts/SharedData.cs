using UnityEngine;
using System.Collections;

/// <summary>
/// Class to share data between scenes
/// </summary>
public static class SharedData
{

    /// <summary>
    /// Sprite that is used when player decided to switch between 1st and 2nd scenes
    /// </summary>
    public static Sprite playerSprite;

    /// <summary>
    /// Has been game won?
    /// </summary>
    public static bool hasWon = false;
    
    /// <summary>
    /// Last generated number
    /// </summary>
    public static int lastNumber;
}
