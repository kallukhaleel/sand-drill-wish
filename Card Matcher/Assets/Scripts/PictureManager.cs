using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PictureManager : MonoBehaviour
{
    [Header("Prefabs & Spawn")]
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.3f, 2.4f);

    [Header("UI Elements")]
    public GameObject GameOverPanel;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI CurrentTimeText;
    public GameObject ScoreUI;
    public GameObject RestartButton;
    public GameObject ExitButton;
    public TextMeshProUGUI HighScoreGameOverText;
    public TextMeshProUGUI BestTimeGameOverText;

    [Header("Audio Clips")]
    public AudioClip matchSound;
    public AudioClip noMatchSound;
    private AudioSource audioSource;

    public enum GameState 
    { 
        NoAction, 
        MovingOnPosition, 
        DeletingPuzzles, 
        FlipBack, 
        Checking, 
        GameEnd
    };

    public enum PuzzleState
    {
        PuzzleRotating,
        CanRotate
    };

    public enum ReveleadState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    public GameState CurrentGameState;
    public PuzzleState CurrentPuzzleState;
    public ReveleadState PuzzleRevealedNumber;

    [HideInInspector]
    public List<Picture> PictureList;

    private Vector2 _offset = new Vector2(1.4f, 1.4f);
    private Vector2 _offset_15Pair = new Vector2(1.08f, 1.22f);
    private Vector2 _offset_20Pair = new Vector2(1.08f, 1.0f);
    private Vector3 _newScaleDown = new Vector3(0.9f, 0.9f, 0.001f);

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    
    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealedPicNumber = 0;
    private int _picToDestroy1;
    private int _picToDestroy2;
    private int comboStreak = 0;
    private bool _courotineStarted = false;

    void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = ReveleadState.NoRevealed;
        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        LoadMaterials();
        audioSource = GetComponent<AudioSource>();


        if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.EPairs10 )
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(4, 5, StartPosition, _offset, false);
            MovePicture(4, 5, StartPosition, _offset);
        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.EPairs15)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(5, 6, StartPosition, _offset_15Pair, false);
            MovePicture(5, 6, StartPosition, _offset_15Pair);
        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.EPairs20)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(5, 8, StartPosition, _offset_20Pair, true);
            MovePicture(5, 8, StartPosition, _offset_20Pair);
        }
    }
    public void CheckPicture()
    {
        CurrentGameState = GameState.Checking;
        _revealedPicNumber = 0;

        for (int id = 0; id < PictureList.Count; id++)
        {
            if (PictureList[id].Revealed && _revealedPicNumber < 2)
            {
                if (_revealedPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealedPicNumber++;
                }
                else if (_revealedPicNumber == 1)
                {
                    _secondRevealedPic = id;
                    _revealedPicNumber++;
                }
            }
        }

        if (_revealedPicNumber == 2)
        {
            bool isMatch = PictureList[_firstRevealedPic].GetIndex() == PictureList[_secondRevealedPic].GetIndex()
                   && _firstRevealedPic != _secondRevealedPic;

            if (isMatch)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _picToDestroy1 = _firstRevealedPic;
                _picToDestroy2 = _secondRevealedPic;

                if (matchSound != null)
                    audioSource.PlayOneShot(matchSound);
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
                comboStreak = 0;

                if (noMatchSound != null)
                    audioSource.PlayOneShot(noMatchSound);
            }
        }

        CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;

        if (CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;

        }
    }

    private IEnumerator HandleCardDestruction()
    {
        PuzzleRevealedNumber = ReveleadState.NoRevealed;
        PictureList[_picToDestroy1].Deactivate();
        PictureList[_picToDestroy2].Deactivate();
       
        _revealedPicNumber = 0;
        comboStreak++;
        int bonus = 10 + (comboStreak * 5);
        ScoreManger.Instance.AddScore(bonus);

        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;

        yield return new WaitForSeconds(1.1f);

        if (AreAllCardsCleared())
        {
            CurrentGameState = GameState.GameEnd;
            GameOver();
        }
        else
        {
            CurrentGameState = GameState.NoAction;
        }
        CurrentPuzzleState = PuzzleState.CanRotate;
    }

    private void GameOver()
    {
        GameOverPanel.SetActive(true);

        float currentTime = Timer.Instance.GetElapsedTime();
        int currentScore = ScoreManger.Instance.GetScore();

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

        if (currentTime < PlayerPrefs.GetFloat("BestTime", float.MaxValue))
        {
            PlayerPrefs.SetFloat("BestTime", currentTime);
        }

        PlayerPrefs.Save();

        FinalScoreText.text = "Your Score: " + ScoreManger.Instance.GetScore();
        string currentTimeUsed = Timer.Instance.GetFormattedTime();
        CurrentTimeText.text = "Your Time: " + currentTimeUsed;


        FindObjectOfType<Timer>().StopTimer();

        if (ScoreUI != null) ScoreUI.SetActive(false);
        if (RestartButton != null) RestartButton.SetActive(false);
        if (ExitButton != null) ExitButton.SetActive(false);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        string formattedTime = bestTime == float.MaxValue ? "N/A" :
            $"{Mathf.Floor(bestTime / 60):00}:{Mathf.FloorToInt(bestTime % 60):00}";

        HighScoreGameOverText.text = "High Score: " + highScore;
        BestTimeGameOverText.text = "Best Time: " + formattedTime;
        
    }

    private bool AreAllCardsCleared()
    {
        foreach (var pic in PictureList)
        {
            if (pic.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }


    private IEnumerator FlipBack()
    {
        _courotineStarted = true;
        yield return new WaitForSeconds(0.5f);

        PictureList[_firstRevealedPic].FlipBack();
        PictureList[_secondRevealedPic].FlipBack();

        PictureList[_firstRevealedPic].Revealed = false;
        PictureList[_secondRevealedPic].Revealed = false;

        PuzzleRevealedNumber = ReveleadState.NoRevealed;
        CurrentGameState = GameState.NoAction;
        _courotineStarted = false;
    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for (var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }

        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    void Update()
    {
        if (CurrentGameState == GameState.DeletingPuzzles && CurrentPuzzleState == PuzzleState.CanRotate)
        {
            StartCoroutine(HandleCardDestruction());
        }
        if (CurrentGameState == GameState.FlipBack)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate && !_courotineStarted)
            {
                StartCoroutine(FlipBack());
            }
        }
        
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicturePrefab.transform.rotation);

                if (scaleDown)
                {
                    tempPicture.transform.localScale = _newScaleDown;
                }

                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var totalPairs = _materialList.Count;
        List<int> materialIndices = new List<int>();

        for (int i = 0; i < totalPairs; i++)
        {
            materialIndices.Add(i);
            materialIndices.Add(i);
        }

        for (int i = materialIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = materialIndices[i];
            materialIndices[i] = materialIndices[j];
            materialIndices[j] = temp;
        }

        for (int i = 0; i < PictureList.Count; i++)
        {
            int matIndex = materialIndices[i];
            Picture pic = PictureList[i];

            pic.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            pic.ApplyFirstMaterial();
            pic.SetSecondMaterial(_materialList[matIndex], _texturePathList[matIndex]);
            pic.SetIndex(matIndex);
            pic.Revealed = false;
        }
    }

    private void MovePicture(int rows, int coloums, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < coloums; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis* Time.deltaTime);
            yield return 0;
        }
    }
}
