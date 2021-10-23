using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterLogic : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    Text textLabel;
    [SerializeField]
    Text fadeText;

    [Header("Text")]
    public TextAsset textFile;
 

    int idx = 0;
    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        textLabel.text = "";
        fadeText.text = "";
        GetTextFromFile(textFile);
        //textLabel.text = textList[0];
        StartCoroutine(ShowText());
    }

    // Update is called once per frame
    void Update()
    {
        //when the letters are all displayed
        // if(idx == textList.Count)
        // {

        // }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        idx = 0;
        var lineData = file.text.Split('\n');

        foreach(var line in lineData)
        {
            textList.Add(line);
        }

    }

    IEnumerator ShowText()
    {
        int i = 0;
        while(i < textList[idx].Length)
        {
            // Color c = fadeText.material.color;
            // c.a = 0;
            // fadeText.material.color = c;
            if(PlayerController.Instance.isWriting)
            {
                fadeText.text += textList[idx][i];
                textLabel.text += textList[idx][i];
                yield return new WaitForSeconds(0.1f);
                //yield return StartCoroutine(FadeShowText()); 
               
                
                i++;
            }
            else
            {
                yield return null;
            }
        }
        idx++;

        gameObject.SetActive(false);
        yield break;
    }

    IEnumerator FadeShowText()
    {
        Color c = fadeText.material.color;
        while(true)
        {
            if(c.a >= 1)
            {
                break;
            }
            c.a += 0.1f;
            fadeText.material.color = c;
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }

    //IEnumerator 


}
