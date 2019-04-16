using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int turnsRemaining;
    //TextMesh turnsText;
    public int xPos, yPos;
    // Start is called before the first frame update
    void Start()
    {
        // This is probably garbage
        //turnsText = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        resetTurns(6);
    }

    

    public void resetTurns(int i)
    {
        turnsRemaining = i;
        //turnsText.text = "" + turnsRemaining;
    }

    public void decrementTurns()
    {
        turnsRemaining--;
        //turnsText.text = "" + turnsRemaining;
    }

    public void EndGame(){
        Debug.Log("Game over!");
        SceneManager.LoadScene("EndScreen");
    }
}
