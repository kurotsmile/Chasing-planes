using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	Rigidbody2D rb;
	public Transform target;
	public Transform arean_dust;
	public GameObject dust;
	public GameObject explosionEffect; 
	public float speed = 6f, rotationSpeed = 120f, dustWait = .05f;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(makingDust());
	}
	
	void FixedUpdate(){
		rb.velocity = transform.up * speed;
		if(target != null){
			Vector2 direction = (Vector2) target.position - rb.position;
			direction.Normalize();
			float angle = Vector3.Cross(direction, transform.up).z;
			rb.angularVelocity = -rotationSpeed * angle;
		}
	}

	IEnumerator makingDust(){
		while(gameObject){
			yield return new WaitForSeconds(dustWait);
			GameObject dustTemp = Instantiate(dust, transform.position, dust.transform.rotation);
			dustTemp.transform.SetParent(this.arean_dust);
			Destroy(dustTemp,4f);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		string tag = other.gameObject.tag;
		if(tag.Equals("plane")){
			blowUpPlane(other.gameObject.transform);
		}

		if(tag.Equals("missile")){
			blowUpSelf();
		}
	}

	void blowUpPlane(Transform plane){
		PlaneBehaviour p = plane.parent.GetComponent<PlaneBehaviour>();
        if (p.is_die == false)
        {
			p.is_die = true;
			GameObject.Find("Game").GetComponent<Game_Handle>().play_sound(2);
			GameObject.Find("Game").GetComponent<Game_Handle>().show_gameover();
			blowUpSelf();
			plane.parent.GetComponent<PlaneBehaviour>().gameOver(transform);
		}
	}

	void blowUpSelf()
	{
		GameObject.Find("Game").GetComponent<Game_Handle>().add_scores();
		GameObject.Find("Game").GetComponent<Game_Handle>().play_sound(1);
		GameObject tempExplosion = Instantiate(explosionEffect, transform.position, dust.transform.rotation);
		Destroy(tempExplosion, 1.2f);
		Destroy(gameObject);
	}
}
