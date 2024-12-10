using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button selectButton = GameObject.Find("SelectButton").GetComponent<Button>();
        selectButton.onClick.AddListener(selectScene);
        
        Button whereButton = GameObject.Find("WhereButton").GetComponent<Button>();
        whereButton.onClick.AddListener(whereScene);
        
        Button unionButton = GameObject.Find("UnionButton").GetComponent<Button>(); 
        unionButton.onClick.AddListener(unionScene);
        
    }

    void selectScene()
    {
        SceneManager.LoadScene("SELECT_Scene");
    }
    
    void whereScene()
    {
        SceneManager.LoadScene("WHERE_Scene");
    }
    
    void unionScene()
    {
        SceneManager.LoadScene("UNION_Scene");
    }
   
}
