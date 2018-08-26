using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour {

	public float speed = -2f;
	SpriteRenderer sr;

	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}

	void Move(){
		rb.velocity = new Vector2(speed, rb.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!other.gameObject.CompareTag("Player")){
			speed = - speed;
			sr.flipX = true;
		}
		else {
			speed = - speed;
			sr.flipX = false;
		}
	}
}
