using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtnHandler : MonoBehaviour
{
    public void ExitGame()
    { 
#if UNITY_EDITOR

    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID

        Application.Quit();
#elif UNITY_IOS

        Debug.Log("iOS does not allow exiting the app manually.");
#else
        Application.Quit();
#endif
    }
}
