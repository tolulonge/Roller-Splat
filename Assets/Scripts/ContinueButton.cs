using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button continueBtn;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        continueBtn = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        continueBtn.onClick.AddListener(LaunchNext);
    }

    private void LaunchNext()
    {
        gameManager.NextLevel();
      
    }
}
