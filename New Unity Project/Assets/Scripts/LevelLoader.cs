using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine

public class LevelLoader : MonoBehaviour
{
    public int iLevelToLoad;
    public string slevelToLoad;

    public bool useIntegerToLoadLevel = false;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameobject;
        if(collisionGameObject.name == "Player")
        {
            LoadScene():
        }
    }

    void LoadScene()
    {

    }
}
