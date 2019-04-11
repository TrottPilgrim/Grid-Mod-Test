```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
GameObject[,] tiles;

/* function checkMatches() iterates through the 2D array "tiles"
 * If the tile is not already flagged for deletion,
 * it checks down and to the right of that tile in the array for tiles of the same type.
 * 
 * 
 */
void checkMatches(){
    for (int x = 0; x < WIDTH - 2; x++){
        for (int y = 0; y < HEIGHT - 2; y++){
            // It will only start checking for matches if this tile is not already part of a match
            if (tiles[x, y].flag = false){
                /*
                int vertGridOffset = 0;
                //Null objects are inherently falsy
                while (tiles[x + vertGridOffset,y]){
                    if (tiles[x, y].type == tiles[x + vertGridOffset, y].type){
                        tiles[x + vertGridOffset, y].flag = true;
                        vertGridOffset++;
                    }
                    else{
                        break;
                    }
                }

                int horzGridOffset = 0;
                while (tiles[x, y + horzGridOffset]){
                    if(tiles[x, y].type = tiles[x, y + horzGridOffset].type){
                        tiles[x. y + horzGridOffset].flag = true;
                        horzGridOffset++;
                    }
                    else{
                        break;
                    }
                }
                */
                if (tiles[x, y].type == tiles[x + 1, y].type &&
                    tiles[x, y].type == tiles[x + 2, y].type)
                {
                    tiles[x, y].flag = true;
                    tiles[x + 1, y].flag = true;
                    tiles[x + 2, y].flag = true;
                }

                if (tiles[x, y].type == tiles[x, y + 1].type &&
                    tiles[x, y].type == tiles[x, y + 2].type)
                {
                    tiles[x, y].flag = true;
                    tiles[x, y + 1].flag = true;
                    tiles[x, y + 2].flag = true;
                }
            }
        }
    }
}
```

```C#
//TileScript.cs
bool flag = false;
int type;
```

# Some constraints for matching:
We **DO NOT** want to check for matches when:
- We're lerping in new tiles
- When we're repopulating the grid

We need to know:
- THe token type
- If we're on an "edge" or not


```c#
//TokenScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Compares this tile's type with two other tiles' types
public bool IsMatch(GameObject gameObject1, GameObject gameObject2)
{
    TokenScript ts1 = gameObject1.GetComponent<TokenScript>();
    TokenScript ts2 = gameObject2.GetComponent<TokenScript>();
    return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type;
}

/*
    1. Check the token type in that spot
    2. Check if we're on the edge of the grid
    3. Check if the first and second token match and if the second and third token match
 */

 public Vector3 startPosition;
public Vector3 destPosition;

public void SetupSlide(Vector2 newDestPos)
{
    inSlide = true;
    startPosition = transform.localPosition;
    destPosition = newDestPos;
}

```

```c#
//GridManager.cs
GameObject tiles[,] = new GameObject[WIDTH, HEIGHT];
public static float slideLerp = -1;
public float lerpSpeed = 0.25f;

public TokenScript HasMatch(){
    for (int x = 0; x < WIDTH; x++)
    {
        for (int y = 0; y < HEIGHT; y++)
        {
            TokenScript temp = tiles[x, y].GetComponent<TokenScript>();

            if (temp != null){
                if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y],tiles[x + 2, y])){
                    return temp;
                }
                if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2])){
                    return temp;
                }
            }
        }
    }
    return null;
}

public bool Repopulate(){
    bool repop = false;
    for (int i = 0; i < WIDTH; i++){
        for 
    }
}

void Update(){
    if (slideLerp < 0 && !Repopulate() && HasMatch()){
        Debug.Log("THERE WAS A MATCH!!")
    }
}

public void RemoveMatches(){
    for (int x = 0; x < WIDTH; x++){
        for(int y = 0; y < HEIGHT; y++){
            TokenScript tokenScript = tokens[x,y].GetComponent<TokenScript>();
            if (tokenScript != null){
                if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y],tiles[x + 2, y])){
                    //emit particles
                    //increase score
                    Destroy(tokens[x, y]);
                    Destroy(tokens[x + 1, y]);
                    Destroy(tokens[x + 2, y]);
                }
                if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2])){
                    //emit particles
                    //increase score
                    Destroy(tokens[x, y]);
                    Destroy(tokens[x, y + 1]);
                    Destroy(tokens[x, y + 2]);
                }
            }
        }
    }
}

```
