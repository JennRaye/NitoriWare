﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Waka : MonoBehaviour {

    Animator animator;

    [Header("Sprites")]
    [SerializeField] Sprite spriteIdle;
    [SerializeField] Sprite spriteLeap;

    // My index
    [HideInInspector] public int _wakaIndex;
    
    // Leap related
    [Header("Leap time")]
    [SerializeField] private float leapTime;
    private float leapAlarm;

    private bool isLeaping = true;
    [HideInInspector] public bool isPassable = true;

    // Tile info
    private Vector2[]   tilePos = new Vector2[2];
    public int          tileCurrent = 0;

    private int start = 0;
    private int finish = 1;
    
	void Start () {
        // The tiles
        tilePos[0] = IcePath_GenerateMap.wakaStart[_wakaIndex];
        tilePos[1] = IcePath_GenerateMap.wakaEnd[_wakaIndex];

        transform.position = tilePos[0];

        // Animator
        animator = transform.Find("Rig").Find("Animation").GetComponent<Animator>();

    }
	
	void Update () {

        Vector2 startTile    = tilePos[start];
        Vector2 finishTile   = tilePos[finish];

        // Leap
        transform.position = Vector3.Lerp(transform.position, finishTile, 0.1f);

        // Set if passable
        Vector2 center = startTile + (finishTile - startTile) / 2;
        bool isFarFromCenter = ((Vector2)transform.position - center).magnitude > 0.1f;

        isPassable = isFarFromCenter;

        // Leap timing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IcePath_WakaAnim")) {
            animator.SetBool("hasLeaped", false);
        }
        
        if (leapAlarm > 0) {

            leapAlarm -= Time.deltaTime;

        } else {

            tileCurrent = tileCurrent == 0 ? 1 : 0;

            start   = tileCurrent;
            finish  = tileCurrent == 0 ? 1 : 0;

            animator.SetBool("hasLeaped", true);

            Transform rig = transform.Find("Rig").transform;
            rig.localScale = new Vector3(rig.localScale.x * -1, 1, 1);

            leapAlarm = leapTime;

        }

    }

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
