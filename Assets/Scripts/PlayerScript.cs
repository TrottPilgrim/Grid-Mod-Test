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
                // Debug.Log("hi");
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
        Debug.Log(tempX + " " + tempY);
        GameObject temp = GridManager.tiles[tempX, tempY];
        // Vector2 tempPos = GridManager.tiles[tempX, tempY].transform.localPosition;
        /*
         * (0, 0), (0, 1), (1, 1), (1, 0)
         * (0, 0) <- (1, 0)
         * (1, 0) <- (1, 1)
         * (1, 1) <- (0, 1)
         * (0, 1) <- (0, 0)
         */

        // GridManager.tiles[tempX, tempY].transform.localPosition = GridManager.tiles[tempX + 1, tempY].transform.localPosition;
        // GridManager.tiles[tempX + 1, tempY].transform.localPosition = GridManager.tiles[tempX + 1, tempY + 1].transform.localPosition;
        // GridManager.tiles[tempX + 1, tempY + 1].transform.localPosition = GridManager.tiles[tempX, tempY + 1].transform.localPosition;
        // GridManager.tiles[tempX, tempY + 1].transform.localPosition = tempPos;
        SwapTransformHelper(GridManager.tiles[tempX, tempY], GridManager.tiles[tempX, tempY + 1]);
        SwapTransformHelper(GridManager.tiles[tempX + 1, tempY], GridManager.tiles[tempX + 1, tempY + 1]);
        SwapTransformHelper(GridManager.tiles[tempX + 1, tempY], GridManager.tiles[tempX, tempY + 1]);


        GridManager.tiles[tempX, tempY] = GridManager.tiles[tempX + 1, tempY];
        GridManager.tiles[tempX + 1, tempY] = GridManager.tiles[tempX + 1, tempY + 1];
        GridManager.tiles[tempX + 1, tempY + 1] = GridManager.tiles[tempX, tempY + 1];
        GridManager.tiles[tempX, tempY + 1] = temp;

        
        gm.ToString();


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

    void SwapTransformHelper(GameObject g1, GameObject g2) {
        Vector2 g1Pos = g1.transform.localPosition;
        g1.transform.localPosition = g2.transform.localPosition;
        g2.transform.localPosition = g1Pos;
    }
}
