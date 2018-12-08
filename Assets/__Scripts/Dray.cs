﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover {
	public enum eMode{idle, move, attack, transition}

	[Header("Set in Inspector")]
	public float speed = 5;
	public float attackDuration = 0.25f;
	public float attackDelay = 0.3f;

	[Header("Set Dynamically")]
	public int dirHeld = -1;
	public int facing = 1;
	public eMode mode = eMode.idle;

	private float timeAtkDone = 0;
	private float timeAtkNext = 0;

	private Rigidbody rigid;
	private Animator anim;
	private InRoom inRm;
	private Vector3[] directions = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down};
	private KeyCode[] keys = new KeyCode[] {KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow};


	void Awake() {
		rigid = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		inRm = GetComponent<InRoom>();
	}
	// Update is called once per frame
	void Update () {
		dirHeld = -1;
		for(int i = 0; i < 4; i++) {
			if(Input.GetKey(keys[i]))
			dirHeld = i;
		}

		if(Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext)
		{
			mode = eMode.attack;
			timeAtkDone = Time.time + attackDuration;
			timeAtkNext = Time.time + attackDelay;
		}

		if(Time.time >= timeAtkDone)
		{
			mode = eMode.idle;
		}
		if(mode != eMode.attack)
		{
			if(dirHeld == -1) 
		{
			mode = eMode.idle;
		} else {
			facing = dirHeld;
			mode = eMode.move;
		}
	}
		
		Vector3 vel = Vector3.zero;
		switch(mode)
		{
			case eMode.attack:
			anim.CrossFade("Dray_Attack_" + facing, 0);
			anim.speed = 0;
			break;

			case eMode.idle:
			anim.CrossFade("Dray_Walk_" + facing, 0);
			anim.speed= 0;
			break;

			case eMode.move:
			vel = directions[dirHeld];
			anim.CrossFade("Dray_Walk_" + facing, 0);
			anim.speed = 1;
			break;
		}
		rigid.velocity = vel * speed;
	}

	
		public int GetFacing() {
			return facing;
		}

		public bool moving {
			get {
				return (mode == eMode.move);
			}
		}

		public float GetSpeed(){
			return speed;
		}

		public float gridMult {
			get{return inRm.gridMult;}
		}

		public Vector2 roomPos {
			get{return inRm.roomPos;}
			set{inRm.roomPos = value;}
		}

		public Vector2 roomNum {
			get{return inRm.roomNum;}
			set{inRm.roomNum = value;}
		}

		public Vector2 GetRoomPosOnGrid(float mult = -1) {
			return inRm.GetRoomPosOnGrid(mult);
		}
}