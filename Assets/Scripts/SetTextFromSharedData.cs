using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets text from shared data
/// </summary>
public class SetTextFromSharedData : MonoBehaviour
{
    /// <summary>
    /// Key that is used for storing shared data
    /// </summary>
    public string sharedDataKey;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Text>().text = typeof(SharedData).GetField(this.sharedDataKey, BindingFlags.Public | BindingFlags.Static).GetValue(null).ToString(); 
    }

}
