using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    List<GameObject> husbandLetters;

    [SerializeField]
    List<GameObject> playerLetters;
    GameObject currentLetter;

    public int letterIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
       
    }

    public void NextLetter()
    {
        if(currentLetter)
        {
            currentLetter.SetActive(false);
        }
        if(letterIdx % 2 == 0)
        {
            currentLetter = husbandLetters[letterIdx / 2];
        }
        else
        {
            currentLetter = playerLetters[letterIdx / 2];
        }
        letterIdx++;
    }
}
