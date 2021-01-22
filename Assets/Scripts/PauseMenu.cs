using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    //public static PauseMenu instance;

    //private void Awake()
    //{
    //    if (PauseMenu.instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //    }

    //    DontDestroyOnLoad(this);
    //}
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
