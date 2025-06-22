using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private int _settings;
    private const int SettingsNumber = 2;

    public enum EPairNumber
    {
        NotSet = 0,
        EPairs10 = 10,
        EPairs15 = 15,
        EPairs20 = 20
    }

    public enum EPuzzleCategories
    {
        Notset,
        Fruits,
        Vegetables
    }

    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    }

    private Settings _gameSettings;

    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _gameSettings = new Settings();
        ResetGameSettings();

    }

    public void SetPairNumber(EPairNumber pairNumber)
    {
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        _gameSettings.PairsNumber = pairNumber;
    }
    public void SetCategory(EPuzzleCategories category)
    {
        if(_gameSettings.PuzzleCategory == EPuzzleCategories.Notset)
        {
            _settings++;

            _gameSettings.PuzzleCategory = category;
        }
    }

    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
        _gameSettings.PuzzleCategory = EPuzzleCategories.Notset;
    }
}
