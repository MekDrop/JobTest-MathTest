using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Switches scene
/// </summary>
public class SceneSwitcher : MonoBehaviour
{

    /// <summary>
    /// Next scene name
    /// </summary>
    public string nextSceneName = "";

    /// <summary>
    /// Switches to next scene
    /// </summary>
    public void SwitchToNextScene()
    {
        SceneManager.LoadScene(this.nextSceneName);
    }

}
