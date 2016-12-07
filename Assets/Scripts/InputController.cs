using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GridSystem gridSystem;

    Vector3 clickPos;
    Vector3 touchPos;


    private bool mouseDrag;
    private bool touchUp;

    void Start()
    {
        touchUp = false;
        mouseDrag = false;
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
        MouseClickController();
#else

        FingerTouchController();
#endif
    }

    void MouseClickController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridSystem.BeginChain(clickPos);
            mouseDrag = true;
        }
        if (mouseDrag)
        {
            clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridSystem.BeginChain(clickPos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //EndChain
            mouseDrag = false;


            gridSystem.isChainOver();
        }
    }

    void FingerTouchController()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            gridSystem.BeginChain(touchPos);
            touchUp = true;
        }
        else if (Input.touchCount == 0)
        {
            if (touchUp)
            {

                gridSystem.isChainOver();
                touchUp = false;
            }

        }
    }
}
