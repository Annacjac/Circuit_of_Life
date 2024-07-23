using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wiring : MonoBehaviour
{
    private string color;
    private int direction; //0=vertical, 1=horizontal, -1=corner, -2=end
    private int corner; //0=top left, 1=top right, 2=bottom left, 3=bottom right
    private int end; //0=up, 1=down, 2=right, 3=left
    private bool soldered;
    public GridLayout grid;
    public TileBase[] redWires;
    public TileBase[] yellowWires;
    public TileBase[] greenWires;
    public TileBase[] blueWires;
    public Tilemap tileMap;
    public Tilemap circuitBoard;
    private Vector3Int previousTile;
    public PowerSource[] powerSources;
    private ArrayList wirePath;
    private bool mouseHeld;

    // Start is called before the first frame update
    void Start()
    {
        wirePath = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
       UserWireInput();
        
    }

    void FirstWire(Vector3Int firstTile, string color){
        this.color = color;

        //check if it's adjacent to a power source
        grid = transform.parent.GetComponentInParent<GridLayout>();

        if(circuitBoard.HasTile(firstTile)){
            for(int i = 0; i < powerSources.Length; i++){
                //Power Source is horizontal
                if(powerSources[i].direction == 1){
                    //Left outlet
                    if(firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).x - 1){
                        wirePath.Add(firstTile);
                        if(color == "red")
                            tileMap.SetTile(firstTile, redWires[5]);
                        // else if(color == "yellow")

                        // else if(color == "green")

                        // else if(color == "blue")

                        previousTile = firstTile;
                    }
                    //top outlets
                    else if(firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).y + 1 || firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).y + 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[2]);
                        previousTile = firstTile;
                    }
                    //bottom outlets
                    else if(firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).y - 1 || firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).y - 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[3]);
                        previousTile = firstTile;
                    }
                    //Right outlet
                    else if(firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).x + 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[4]);
                        previousTile = firstTile;
                    }
                }

                //Power source is vertical
                else{
                    //top outlet
                    if(firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).y + 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[2]);
                        previousTile = firstTile;
                    }
                    //left outlets
                    else if(firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).x - 1 || firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).x - 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[5]);
                        previousTile = firstTile;
                    }
                    //bottom outlet
                    else if(firstTile.y == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).y - 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[3]);
                        previousTile = firstTile;
                    }
                    //right outlets
                    else if(firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(0).position).x + 1 || firstTile.x == grid.WorldToCell(powerSources[i].transform.GetChild(1).position).x + 1){
                        wirePath.Add(firstTile);
                        tileMap.SetTile(firstTile, redWires[4]);
                        previousTile = firstTile;
                    }
                }
            }

        }
    }

    void DrawWires(Vector3Int targetTile){
        if(circuitBoard.HasTile(targetTile)){
            if(color == "red"){
                //Previous is down facing end
                if(tileMap.GetTile(previousTile) == redWires[3]){

                    //wire turns right
                    //turn prev tile into bottom left corner, turn current tile to right facing end
                    if(targetTile.x == previousTile.x + 1){
                        tileMap.SetTile(previousTile, redWires[12]);
                        tileMap.SetTile(targetTile, redWires[4]);
                        wirePath.Add(targetTile);
                    }

                    //wire turns left
                    //turn prev tile into bottom right corner, turn current tile to left facing end
                    else if(targetTile.x == previousTile.x - 1){
                        tileMap.SetTile(previousTile, redWires[13]);
                        tileMap.SetTile(targetTile, redWires[5]);
                       wirePath.Add(targetTile);
                    }

                    //wire continues down
                    //turn prev tile into vertical straight, turn current tile to down facing end
                    else if(targetTile.y == previousTile.y - 1){
                        tileMap.SetTile(previousTile, redWires[0]);
                        tileMap.SetTile(targetTile, redWires[3]);
                        wirePath.Add(targetTile);
                    }
                }

                //Prev tile is up facing end
                else if(tileMap.GetTile(previousTile) == redWires[2]){
                
                    //wire turns right
                    //turn prev into upper left corner, turn current to right facing end
                    if(targetTile.x == previousTile.x + 1){
                        tileMap.SetTile(previousTile, redWires[10]);
                        tileMap.SetTile(targetTile, redWires[4]);
                        wirePath.Add(targetTile);
                    }

                    //wire turns left
                    //turn prev into upper right corner, turn current to left facing end
                    else if(targetTile.x == previousTile.x - 1){
                        tileMap.SetTile(previousTile, redWires[11]);
                        tileMap.SetTile(targetTile, redWires[5]);
                        wirePath.Add(targetTile);
                    }

                    //wire continues up
                    //turn prev into vertical straight, turn current to up facing end
                    else if(targetTile.y == previousTile.y + 1){
                        tileMap.SetTile(previousTile, redWires[0]);
                        tileMap.SetTile(targetTile, redWires[2]);
                       wirePath.Add(targetTile);
                    }
                }

                //Prev is right facing end
                else if(tileMap.GetTile(previousTile) == redWires[4]){

                    //Wire continues right
                    //turn prev into horizontal straight, turn current to right facing end
                    if(targetTile.x == previousTile.x + 1){
                        tileMap.SetTile(previousTile, redWires[1]);
                        tileMap.SetTile(targetTile, redWires[4]);
                        wirePath.Add(targetTile);
                    }

                    //wire turns up
                    //turn prev into bottom right corner, turn current to up facing end
                    else if(targetTile.y == previousTile.y + 1){
                        tileMap.SetTile(previousTile, redWires[13]);
                        tileMap.SetTile(targetTile, redWires[2]);
                        wirePath.Add(targetTile);

                    }

                    //wire turns down
                    //turn prev into upper right corner, turn current to down facing end
                    else if(targetTile.y == previousTile.y - 1){
                        tileMap.SetTile(previousTile, redWires[11]);
                        tileMap.SetTile(targetTile, redWires[3]);
                        wirePath.Add(targetTile);
                    }
                }

                //Prev is left facing end
                else if(tileMap.GetTile(previousTile) == redWires[5]){
                    //wire continues left
                    //turn prev into horizontal straight, turn current to left facing end
                    if(targetTile.x == previousTile.x - 1){
                        tileMap.SetTile(previousTile, redWires[1]);
                        tileMap.SetTile(targetTile, redWires[5]);
                        wirePath.Add(targetTile);
                    }

                    //wire turns up
                    //turn prev into bottom left corner, turn current to up facing end
                    else if(targetTile.y == previousTile.y + 1){
                        tileMap.SetTile(previousTile, redWires[12]);
                        tileMap.SetTile(targetTile, redWires[2]);
                        wirePath.Add(targetTile);
                    }

                    //wire turns down
                    //turn prev into upper left corner, turn current to down facing end
                    else if(targetTile.y == previousTile.y - 1){
                        tileMap.SetTile(previousTile, redWires[10]);
                        tileMap.SetTile(targetTile, redWires[3]);
                        wirePath.Add(targetTile);
                    }
                }
            }
        }
        string wirePathString = "";
        for(int i = 0; i < wirePath.Count; i++){
            wirePathString = wirePathString + " " + wirePath[i];
        }
        Debug.Log(wirePathString);
        previousTile = targetTile;
    }

    void DeleteWires(Vector3Int targetTile){
        
        tileMap.SetTile((Vector3Int)wirePath[wirePath.Count-1], null);
        wirePath.RemoveAt(wirePath.Count-1);
        previousTile = targetTile;
    }

    void ClearBoard(){
        tileMap.ClearAllTiles();
    }



    void UserWireInput(){
        // Event m_Event = Event.current;
        // Vector3 mousePosition = Input.mousePosition;
        Vector3Int targetTile;

        if(Input.GetMouseButtonDown(0)){
            mouseHeld = true;
        }
        if(Input.GetMouseButtonUp(0)){
            mouseHeld = false;
        }

        if(mouseHeld){
            Vector3Int currentTile;
            targetTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(wirePath.Count == 0){
                FirstWire(targetTile, "red");
            }
            else{
                DrawWires(targetTile);
                // if(grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)) == previousTile){
                     
                //      DeleteWires(targetTile);
                // }
                // else{
                //     DrawWires(targetTile);
                // }
                
            }
        }
        
    }

}
