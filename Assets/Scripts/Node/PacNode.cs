using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacNode : Node
{
    // Start is called before the first frame update

    public bool showSprite = true;
    void Start()
    {
        if (model.isWalkable)
        {
            if (!showSprite)
                OpenCell.SetActive(false);
        }
        else
        {
            OpenCell.SetActive(false);
            ClosedCell.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
