using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
