using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public GUIStyle ClockStyle;

    private float _timer;
    private bool _stopTimer;
    private bool _hideTimer;

    private const float VirtualHeight = 854.0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _stopTimer = false;
        _hideTimer = false;
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
        if (_hideTimer) return;

        Matrix4x4 _oldMatrix = GUI.matrix;
        float scale = Screen.height / VirtualHeight;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1.0f));

        float _minutes = Mathf.Floor(_timer / 60);
        float _seconds = Mathf.RoundToInt(_timer % 60);

        GUIStyle miniStyle = new GUIStyle(ClockStyle)
        {
            fontSize = 25,
            normal = { textColor = Color.black },
            alignment = TextAnchor.UpperLeft
        };

        GUI.Label(new Rect(760, 15, 70, 40), $"{_minutes:00}:{_seconds:00}", miniStyle);
        GUI.matrix = _oldMatrix;
    }

    public void StopTimer()
    {
        _stopTimer = true;
        _hideTimer = true;
    }

    public float GetElapsedTime()
    {
        return _timer;
    }

    public string GetFormattedTime()
    {
        return $"{Mathf.FloorToInt(_timer / 60):00}:{Mathf.FloorToInt(_timer % 60):00}";
    }

}
