using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class canvasManager : MonoBehaviour
{

    [SerializeField] Canvas canvasSkill;
    // Start is called before the first frame update
    void Start()
    {
        canvasSkill.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
        
    }

    //Simple Toggle function
    //By pressing escape you are able to turn the skill tree on and off. 
    void Toggle()
    {
        bool currentState = canvasSkill.gameObject.activeSelf;
     
        canvasSkill.gameObject.SetActive(!currentState);
        
    }
}
