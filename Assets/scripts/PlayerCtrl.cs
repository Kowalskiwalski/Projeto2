﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public float horizontalSpeed = 10f;

	public float jumpSpeed =600f;

	Rigidbody2D rb;

	SpriteRenderer sr;

	Animator anim;

	bool isjumping = false;

	public Transform feet;

	public float feetWidth = 0.5f;

	public float feetHeight = 0.1f;

	public bool isGrounded;

	public LayerMask whatisGround;

	bool canDoubleJump = false; 

	public float delayForDoubleJump = 0.2f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr= GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(feet.position, new Vector3(feetWidth, feetHeight, 0f));
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position.y < GM.instance.yMinLive){
			GM.instance.KillPlayer();
		}
		
		isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y ), new Vector2(feetWidth, feetHeight), 360.0f, whatisGround );

		float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1: esquerda, 1: direita
		float horizontalPlayerSpeed = horizontalSpeed * horizontalInput;
		if (horizontalPlayerSpeed != 0) {
			MoveHorizontal(horizontalPlayerSpeed);
		}
		else {
			StopMovingHorizontal();
		}

		if(Input.GetButtonDown("Jump")){
			Jump();
		}

		ShowFalling();
	}

	void MoveHorizontal(float speed) {
		rb.velocity = new Vector2(speed, rb.velocity.y);

		if(speed < 0f) {
			sr.flipX = false;
		}
		else if (speed > 0f) {
			sr.flipX = true;
		}

		if (!isjumping){
			anim.SetInteger("State", 2); 
		}
	}

	void StopMovingHorizontal(){
		rb.velocity = new Vector2(0f, rb.velocity.y);
		if (!isjumping){
		anim.SetInteger("State", 0);
		}
	}

	void ShowFalling(){
		if(rb.velocity.y < 0f){
			anim.SetInteger("State", 3);
		}
	}

	void Jump(){
		if(isGrounded){
		isjumping = true;
		rb.AddForce(new Vector2(0f, jumpSpeed));
		anim.SetInteger("State", 1);

		Invoke("EnableDoubleJump", delayForDoubleJump);
		}

		if(canDoubleJump && !isGrounded){
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(0f, jumpSpeed));
			anim.SetInteger("State", 1);
			canDoubleJump = false;
		}
	}

	void EnableDoubleJump(){
		canDoubleJump = true;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")){
			isjumping = false; 
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag("Coin")){
			Destroy(other.gameObject);
		}
	}
}
