﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //public static GridManager Instance;
    public static GameObject[,] tiles;
    public GameObject tilePrefab;
    public GameObject playerFab;
    public const int WIDTH = 11;
    public const int HEIGHT = 7;

    public const float xOffset = WIDTH / 2f - 0.5f;
    public const float yOffset = HEIGHT / 2f - 0.5f;

    GameObject gridHolder;
    PlayerScript playerScript;

    public ParticleSystem explosion1;
    public ParticleSystem explosion2;
    public ParticleSystem explosion3;
    public static float slideLerp = -1f;
    public float lerpSpeed;
    
    //Score + text
    private int score;
    public Text scoreText;

    void Start()
    {
        lerpSpeed = 0.25f;
        
        tiles = new GameObject[WIDTH, HEIGHT];
        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -0.5f, 0);
        
        for (int i = 0; i < WIDTH; i++){
            for (int j = 0; j < HEIGHT; j++){
                GameObject newTile = Instantiate(tilePrefab);
                newTile.name = i + "," + j;
                newTile.transform.parent = gridHolder.transform;

                //WHY does this matrix go from upper right to bottom left. Is this supposed to happen?
                //I know why, it's because then you can just use Input.GetAxis for movement.
                newTile.transform.localPosition = new Vector2(WIDTH - i - xOffset, HEIGHT - j - yOffset);
                //newTile.transform.localPosition = new Vector2( i - 1, HEIGHT - j - yOffset);

                tiles[i, j] = newTile;
                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
            }
        }
        score = 0;
        //Changes tiles that are part of matches as a part of board generation. Potentially will run forever
        //Also probably not cache friendly or something
        while (HasMatch()){
            TileScript temp = HasMatch();
            temp.SetSprite(Random.Range(0, temp.tileColors.Length));
        }
        
        //Initializing the player object
        GameObject player = Instantiate(playerFab);
        playerScript = player.GetComponent<PlayerScript>();
        player.transform.parent = gridHolder.transform;
        player.transform.localPosition = new Vector2(WIDTH - xOffset, HEIGHT - yOffset);
    }

    
    void Update()
    {
        if (slideLerp < 0 && !Repopulate() && HasMatch()){
            RemoveMatches();
        }
        else if (slideLerp >= 0)
        {
            slideLerp += Time.deltaTime / lerpSpeed;
            //Debug.Log(slideLerp);
            // Debug.Log(Time.deltaTime + " " + slideLerp + " Lerp speed: " + lerpSpeed);

            if (slideLerp >= 1)
                slideLerp = -1;
        }
    }
    //Hasmatch returns an object that has a matching object vertically or horizontally
    public TileScript HasMatch(){
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                TileScript temp = tiles[x, y].GetComponent<TileScript>();
                if (temp is object){
                    if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y], tiles[x + 2, y]))
                        return temp;
                    if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2]))
                        return temp;
                }
            }
        }
        return null;
    }

    public void RemoveMatches(){
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                TileScript temp = tiles[x, y].GetComponent<TileScript>();
                if (temp is object){
                    if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        explosion1.transform.position = tiles[x, y].transform.position;
                        explosion1.Play();
                        explosion2.transform.position = tiles[x + 1, y].transform.position;
                        explosion2.Play();
                        explosion3.transform.position = tiles[x + 2, y].transform.position;
                        explosion3.Play();
                        score += 1;
                        //Debug.Log("Horizontal Match " + x + " " + y);
                        Destroy(tiles[x, y]);
                        Destroy(tiles[x + 1, y]);
                        Destroy(tiles[x + 2, y]);
                        scoreText.text = "SCORE: " + score;
                        //playerScript.resetTurns(6);
                    }
                    if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2]))
                    {   
                        explosion1.transform.position = tiles[x, y].transform.position;
                        explosion1.Play();
                        explosion2.transform.position = tiles[x, y + 1].transform.position;
                        explosion2.Play();
                        explosion3.transform.position = tiles[x, y + 2].transform.position;
                        explosion3.Play();
                        score += 1;
                        //Debug.Log("Vertical Match " + x + " " + y);
                        Destroy(tiles[x, y]);
                        Destroy(tiles[x, y + 1]);
                        Destroy(tiles[x, y + 2]);
                        scoreText.text = "SCORE: " + score;
                        //playerScript.resetTurns(6);
                    }
                }
            }
        }
    }

    public bool Repopulate(){
        bool repop = false;
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                //Check if there is an empty spot in the array
                if (tiles[x, y] == null){
                    //The game knows we're about to repopulate
                    repop = true;
                    //Check if we're on the top row of the grid
                    if (y == 0){
                        //If so, make a token
                        tiles[x, y] = Instantiate(tilePrefab);
                        tiles[x, y].name = x + "," + y;
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                        tiles[x, y].transform.parent = gridHolder.transform;
                        tiles[x, y].transform.localPosition = new Vector2 (WIDTH - x - xOffset, HEIGHT - y - yOffset);

                    }
                    else {
                        slideLerp = 0.0f;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        if (tileScript != null){
                            tileScript.SetupSlide(new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset));
                            tiles[x, y - 1] = null;
                            //tiles[x, y].transform.localPosition = Vector3.Lerp(tileScript.startPosition, tileScript.destPosition, lerpSpeed);
                        }
                        //Changes the PlayerScript xPos and yPos to the position it lerps down to
                        // if (tileScript.type == -1){
                        //     PlayerScript temp2 = tileScript.GetComponentInParent<PlayerScript>();
                        //     temp2.xPos = x;
                        //     temp2.yPos = y;
                        //     //Debug.Log(temp2.xPos + " " + temp2.yPos);
                        // }
                    }
                }
            }
        }
        return repop;
    }

    public override string ToString()
    {
        Debug.Log("matrix: ");
        for (int y = 0 ; y < HEIGHT; y++)
        
        {
            StringBuilder line = new StringBuilder();
            for (int x = WIDTH - 1; x >= 0; x--)
            {
                line.Append(tiles[x, y].GetComponent<TileScript>().type + "\t");
            }
            Debug.Log(line + "\n");
        }
        return "";
    }
}
