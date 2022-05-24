using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiarScene : MonoBehaviour
{
    public static int dificultad;
    
    public void elegirDificultad(int dif)
    {

      dificultad = dif;

    }

    public void LoadScene(string sceneName)
    {

      SceneManager.LoadScene(sceneName);
    
    }
}
