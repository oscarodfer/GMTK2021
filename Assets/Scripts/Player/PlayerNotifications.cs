using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PlayerNotifications : MonoBehaviour
{

    [SerializeField] TextMeshPro tmp;
     
    float timeInScreen = 3;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeInScreen * 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, Mathf.Lerp(1, 0, timer / timeInScreen));
    }

    public void ShowText(string text, float timeInSeconds)
    {
        timeInScreen = timeInSeconds;
        tmp.text = text;
        timer = 0;
    }
}
