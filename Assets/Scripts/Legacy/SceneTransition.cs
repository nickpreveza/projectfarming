using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public GameManager.HempScene nextScene;
    [HideInInspector]
    public string currentScene { get; private set; }
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag.CompareTo("Player") == 0) && (nextScene != GameManager.HempScene.None))
        {
            GameManager.Instance.LoadNextScene(nextScene);
        }
    }


}
