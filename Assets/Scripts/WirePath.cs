using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WirePath : MonoBehaviour
{
    public ArrayList path;
    public string color;
    public bool soldered;
    public bool active;

    public WirePath(string color){
        this.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        path = new ArrayList();
        soldered = false;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
