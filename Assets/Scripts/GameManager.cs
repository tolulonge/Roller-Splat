using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    // Start is called before the first frame update

    private GroundPiece[] allGroundPieces;
    public ParticleSystem explosionParticle;
    public Button continueBtn;
    public TextMeshProUGUI congratsText;

    void Start()
    {
        SetupNewLevel();
        continueBtn = GameObject.Find("Canvas").transform.Find("ContinueBtn").GetComponent<Button>();
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;

        }else if(singleton != this)
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }


    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {

            if(allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            continueBtn.gameObject.SetActive(true);
            if(SceneManager.GetActiveScene().buildIndex == 4)
            {
                congratsText.gameObject.SetActive(true);
            }
            explosionParticle.Play();
        }
    }

    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(0);
        
        }
        else
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       
        
    }

    private void Update()
    {
        if(continueBtn == null || explosionParticle == null)
        {
            continueBtn = GameObject.Find("Canvas").transform.Find("ContinueBtn").GetComponent<Button>();
            
            explosionParticle = GameObject.Find("Passed").transform.Find("Explosion").GetComponent<ParticleSystem>();
        }

        if(congratsText == null && SceneManager.GetActiveScene().buildIndex == 4)
        {
            congratsText = GameObject.Find("Canvas").transform.Find("congratsText").GetComponent<TextMeshProUGUI>();
        }
        
    }

}
