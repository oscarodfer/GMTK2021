using Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
   
    // Start is called before the first frame update

    public void LoadScene(string name)
    {
        Debug.Log("Load scene before: " + Time.timeScale);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        Debug.Log("Load scene after: " + Time.timeScale);
    }


}
