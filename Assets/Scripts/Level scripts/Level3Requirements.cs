using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Requirements : MonoBehaviour
{
    public Wiring wiring;
    public Lightbulb[] lightbulbs;
    public Switch[] switches;
    public bool[] requirementMet;
    string[] requirements;
    
    void Awake(){
        requirements[0] = "1. Lightbulb 1 must be ";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetRequirements(){
        return requirements;
    }

    public bool[] GetRequirementsMet(){
        return requirementMet;
    }
}
