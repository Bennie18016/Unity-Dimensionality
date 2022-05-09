 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSplash : MonoBehaviour
{
    float finishSplash;
    float splashTime;

    void Start()
    {
        finishSplash = 3;
        splashTime = 0;
    }

    void Update()
    {
        splashTime += 1 * Time.deltaTime;

        if(splashTime >= finishSplash){
            SceneManager.LoadScene(1);
        }
    }
}
