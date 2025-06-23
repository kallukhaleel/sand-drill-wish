using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton : MonoBehaviour
{
    public enum EbuttonType
    {
        NotSet,
        PairNumberBtn,
        PuzzleCategoryBtn,
    }

    [SerializeField] public EbuttonType buttonType = EbuttonType.NotSet;
    [HideInInspector] public GameSettings.EPairNumber PairNumber = GameSettings.EPairNumber.NotSet;
    [HideInInspector] public GameSettings.EPuzzleCategories PuzzleCategories = GameSettings.EPuzzleCategories.Notset;


    void Start()
    {
        
    }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameButton>();

        switch(comp.buttonType)
        {
            case EbuttonType.PairNumberBtn:
                GameSettings.Instance.SetPairNumber(comp.PairNumber);
                break;
            case EbuttonType.PuzzleCategoryBtn:
                GameSettings.Instance.SetCategory(comp.PuzzleCategories);
                break;
        }

        if (GameSettings.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
