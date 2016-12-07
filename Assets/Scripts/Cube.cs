using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour{
    
    public enum CubeColor
    {
        Gray, Red, Green, Yellow, Blue, Purple
    }

    public CubeColor cubeColor;
    
    public Vector2 order;
    public Vector2 position;


    public void Kill()
    {
        Destroy(this.gameObject);
    }

  
}
