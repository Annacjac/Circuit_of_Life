using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Switch : MonoBehaviour
{

    bool isOn;
    public Transform worldPosition;
    Vector3Int cellPosition;
    public GridLayout grid;
    public Sprite[] switchSprite;
    public SpriteRenderer sprite;
    public int direction; //0 = horizontal, 1 = vertical
    Vector3Int targetTile;
    // Start is called before the first frame update
    void Start()
    {
        cellPosition = grid.WorldToCell(worldPosition.position);
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        ClickSwitch();
    }

    void ToggleOnOff(){
        if(isOn){
            isOn = false;
            if(direction == 0)
                sprite.sprite = switchSprite[0];
            else if(direction == 1)
                sprite.sprite = switchSprite[1];
        }
        else{
            isOn = true;
            if(direction == 0)
                sprite.sprite = switchSprite[2];
            else if(direction == 1)
                sprite.sprite = switchSprite[3];
        }
    }

    void ClickSwitch(){
        if(Input.GetMouseButtonUp(0)){
            if(targetTile == cellPosition){
            ToggleOnOff();
        }
        }
        
    }

}
