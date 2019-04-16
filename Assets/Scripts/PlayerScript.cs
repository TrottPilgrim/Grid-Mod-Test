using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int turnsRemaining;
    //TextMesh turnsText;
    //public int xPos, yPos;
    public Vector2 playerPos;
    public GridManager gm;
    void Start()
    {
        // This is probably garbage
        gm = GameObject.Find("GameManager").GetComponent<GridManager>();
        //turnsText = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        resetTurns(100);
    }

    void Update ()
    {   if (GridManager.slideLerp < 0)
        {
            if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(1, 0);
            }
            if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("hi");
                Rotate();
            }
        }
    }
    
    void Move (int x, int y)
    {
        Vector2 oldLoc = new Vector2(playerPos.x, playerPos.y);
        Vector2 newLoc = new Vector2(playerPos.x + x, playerPos.y + y);
        if ((int) newLoc.x < GridManager.WIDTH - 1 &&
            (int) newLoc.x >= 0 &&
            (int) newLoc.y < GridManager.HEIGHT - 1 &&
            (int) newLoc.y >= 0)
        {
            playerPos = newLoc;
            // Debug.Log("new log" + newLoc.x + " " + newLoc.y);
            Vector2 newLocPos = new Vector2(GridManager.WIDTH - newLoc.x - GridManager.xOffset, GridManager.HEIGHT - newLoc.y - GridManager.yOffset);
            this.transform.localPosition = newLocPos;
        }
    }

    public void resetTurns(int i)
    {
        turnsRemaining = i;
        //turnsText.text = "" + turnsRemaining;
    }

    void Rotate()
    {   
        //The player is anchored on this block
        int tempX = (int) playerPos.x;
        int tempY = (int) playerPos.y;
        GameObject temp = gm.tiles[tempX, tempY];
        Vector2 tempPos = gm.tiles[tempX, tempY].transform.localPosition;
        /*
         * (0, 0), (0, 1), (1, 1), (1, 0)
         */
        Debug.Log("" + tempX + "," + tempY + " swaps with " + (tempX + 1) + "," + tempY);
        gm.tiles[tempX, tempY].transform.localPosition = gm.tiles[tempX + 1, tempY].transform.localPosition;
        gm.tiles[tempX, tempY] = gm.tiles[tempX + 1, tempY];
        
        gm.tiles[tempX + 1, tempY].transform.localPosition = gm.tiles[tempX + 1, tempY + 1].transform.localPosition;
        gm.tiles[tempX + 1, tempY] = gm.tiles[tempX + 1, tempY + 1];
        
        gm.tiles[tempX + 1, tempY + 1].transform.localPosition = gm.tiles[tempX, tempY + 1].transform.localPosition;
        gm.tiles[tempX + 1, tempY + 1] = gm.tiles[tempX, tempY + 1];
        
        gm.tiles[tempX, tempY + 1].transform.localPosition = tempPos;
        gm.tiles[tempX, tempY + 1] = temp;


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
