using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;



public class LevelUI : MonoBehaviour
{

    public TextMeshProUGUI mode;
    public TextMeshProUGUI componentName;
    public Button nextButton;
    string name;
    public ObjectsAndData data;


    // Start is called before the first frame update
    void Start()
    {
        //nextButton.SetEnabled(false);
        mode.text = "Mode: Wiring";
        this.name = "";
        componentName.text = "";
        data = GameObject.Find("Data").GetComponent<ObjectsAndData>();
    }

    // Update is called once per frame
    void Update()
    {
        
        SetModeText();
        ShowComponentName();
        
        componentName.text = this.name;

    }

    public void ResetLevel(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    void SetModeText(){
        string modeString = "Mode: ";
        if(data.wiring.GetMode() == 0){
            modeString += "Wiring";
        }
        else if(data.wiring.GetMode() == 1){
            modeString += "Soldering";
        }
        else{
            modeString += "Wire Removal";
        }
        mode.text = modeString;
    }

    

    void ShowComponentName(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellMousePos = data.grid.WorldToCell(mousePos);
        componentName.transform.position = new Vector3(mousePos.x + 0.3f, mousePos.y-0.25f, componentName.transform.position.z);
        
        foreach(KeyValuePair<Vector3Int, Object> comp in data.componentDictionary){
            if(cellMousePos == comp.Key){
                this.name = comp.Value.name;
                break;
            }
            else{
                this.name = "";
            }
        }

        componentName.text = this.name;

    }


}
