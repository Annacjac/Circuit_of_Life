using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    private int totalWattage;
    private int wattagePerOutlet;
    private Vector3Int powerPosition;
    public int direction; //0=vertical, 1=horizontal

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SplitPower(int numWires){
        wattagePerOutlet = totalWattage/numWires;
    }
}
