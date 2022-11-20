using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private RectTransform crosshair;

    public GameObject player;

    public float aimSize = 25f;
    public float idleSize = 50f;
    public float walkSize = 75f;
    public float runJumpSize = 125f;
    public float currentSize = 50f;
    public float speed = 10f;

    private void Update()
    // Depending on the player's movement, adjust the size of the crosshair accordingly
    {
        if (Aiming)
        {
            currentSize = Mathf.Lerp(currentSize, aimSize, Time.deltaTime * speed);
        }
        else if (Walking)
        {
            currentSize = Mathf.Lerp(currentSize, walkSize, Time.deltaTime * speed);
        }
        else if (Running)
        {
            currentSize = Mathf.Lerp(currentSize, runJumpSize, Time.deltaTime * speed);
        }
        else 
        {
            currentSize = Mathf.Lerp(currentSize, idleSize, Time.deltaTime * speed);
        }

        crosshair.sizeDelta = new Vector2(currentSize, currentSize);
    }

    public bool Aiming
    {
        get
        {
            if (Input.GetMouseButton(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    bool Walking
    {
        get
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    bool Running
    {
        get
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}
