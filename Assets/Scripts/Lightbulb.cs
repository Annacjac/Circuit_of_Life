using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    private bool isOn;
    public Vector3Int lightPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnOn(){
        isOn = true;

    }

    void TurnOff(){
        isOn = false;
    }
}
