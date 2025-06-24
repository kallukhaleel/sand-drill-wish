using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Button Clicked: Trying to load scene " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGameSettings()
    {
        GameSettings.Instance.ResetGameSettings();
    }
}
