using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text text;

    void Update()
    {
        text.text = "Score: "+GameData.Instance.Score.ToString();
    }
}
