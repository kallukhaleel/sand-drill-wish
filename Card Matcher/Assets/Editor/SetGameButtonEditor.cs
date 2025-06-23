using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetGameButton))]
[CanEditMultipleObjects]
[System.Serializable]

public class SetGameButtonEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SetGameButton myScript = target as SetGameButton;

        switch (myScript.buttonType)
        {
            case SetGameButton.EbuttonType.PairNumberBtn:
                myScript.PairNumber = (GameSettings.EPairNumber) EditorGUILayout.EnumPopup("Pair Number", myScript.PairNumber);
                break;

            case SetGameButton.EbuttonType.PuzzleCategoryBtn:
                myScript.PuzzleCategories = (GameSettings.EPuzzleCategories)EditorGUILayout.EnumPopup("Puzzle Category", myScript.PuzzleCategories);
                break;
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

    }
}
