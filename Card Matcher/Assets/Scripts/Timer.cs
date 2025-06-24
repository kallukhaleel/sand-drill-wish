using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GUIStyle ClockStyle;

    private float _timer;
    private float _minutes;
    private float _seconds;

    // widht and height of our screen
    private const float VirtualWidth = 480.0f;
    private const float VirtualHeight = 854.0f;

    private bool _stopTimer;
    private Matrix4x4 _matrix;
    private Matrix4x4 _oldMatrix;


    void Start()
    {
        _stopTimer = false;
        _matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / VirtualWidth, Screen.height / VirtualHeight, 1.0f ));
        _oldMatrix = GUI.matrix;
    }

    void Update()
    {
        if (!_stopTimer)
        {
            _timer += Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        GUI.matrix = _matrix;

        _minutes = Mathf.Floor(_timer / 60);
        _seconds = Mathf.RoundToInt(_timer % 60);

        GUI.Label(new Rect(190, 10, 50, 40), $"{_minutes:00}:{_seconds:00}", ClockStyle);

        GUI.matrix = _oldMatrix;
    }
}
