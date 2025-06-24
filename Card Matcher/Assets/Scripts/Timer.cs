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

    private const float VirtualHeight = 854.0f;

    private bool _stopTimer;
    private Matrix4x4 _oldMatrix;


    void Start()
    {
        _stopTimer = false;
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
        Matrix4x4 _oldMatrix = GUI.matrix;

        float scale = Screen.height / VirtualHeight;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1.0f));

        _minutes = Mathf.Floor(_timer / 60);
        _seconds = Mathf.RoundToInt(_timer % 60);

        GUIStyle miniStyle = new GUIStyle(ClockStyle);
        miniStyle.fontSize = 21;
        miniStyle.normal.textColor = Color.black;
        miniStyle.alignment = TextAnchor.UpperLeft;

        GUI.Label(new Rect(820, 10, 50, 40), $"{_minutes:00}:{_seconds:00}", miniStyle);

        GUI.matrix = _oldMatrix;
    }

    public void StopTimer()
    {
        _stopTimer = true;
    }

    public float GetElapsedTime()
    {
        return _timer;
    }
}
