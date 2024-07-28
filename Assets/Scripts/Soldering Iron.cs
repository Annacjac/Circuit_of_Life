using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolderingIron : MonoBehaviour
{
    public GridLayout grid;
    public Wiring wiring;
    private bool isHeld;
    private Vector3Int defaultPosition1;
    private Vector3Int defaultPosition2;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition1 = grid.WorldToCell(transform.GetChild(0).position);
        defaultPosition2 = grid.WorldToCell(transform.GetChild(1).position);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForInput();
    }

    void CheckForInput(){
        Vector3Int mousePosition = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0) && !isHeld && (mousePosition == defaultPosition1 || mousePosition == defaultPosition2)){
            isHeld = true;
            //PickUp();
        }
        else if(Input.GetMouseButtonDown(0) && isHeld){
            isHeld = false;
            //PutDown();
        }
        //Debug.Log(isHeld);
        
    }

    // void PickUp(){
    //     transform.GetChild(1).position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // }

    // void PutDown(){
    //     transform.GetChild(1).position = grid.CellToWorld(defaultPosition2);
    // }


}
