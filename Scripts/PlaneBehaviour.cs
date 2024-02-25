using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour {

	public Camera cam;
	public GameObject obj_tip_help;
	public Transform plane;
	public GameObject brokenPlane, canvas;
	public Transform leftPoint, rightPoint, forwardPoint;
	public Rigidbody2D rb;
	public float speed = 5f, rotateSpeed = 50f;
	public bool is_die = false;
	bool isGameOver;
	bool is_move;
	float timer_reset_move = 0f;

	void Start () {
		if(cam == null) cam = Camera.main;
		this.rotatePlane(0f);
	}
	
	void FixedUpdate () {
		if(!isGameOver){
			movePlane();
		}
	}

    void Update()
    {
        if (this.is_move)
        {
			this.timer_reset_move+=1f*Time.deltaTime;
            if (this.timer_reset_move > 1f)
            {
				this.rotatePlane(0);
				this.is_move = false;
				this.timer_reset_move = 0;
			}
        }
    }

	void movePlane(){
		rb.velocity = transform.up * speed;
	}

	public void rotatePlane(float x){	
		float angle;
		Vector2 direction = new Vector2(0,0);

		if(x < 0) direction = (Vector2) leftPoint.position - rb.position;
		if(x > 0) direction = (Vector2) rightPoint.position - rb.position;
		
		direction.Normalize();
		angle = Vector3.Cross(direction, transform.up).z;
		if(x != 0) rb.angularVelocity = -rotateSpeed * angle;
		else rb . angularVelocity = 0;
		angle = Mathf.Atan2(forwardPoint.position.y - plane.transform.position.y, forwardPoint.position.x - plane.transform.position.x) * Mathf.Rad2Deg;
		plane.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
	}

	public void gameOver(Transform missile){
		GetComponentInChildren<SpriteRenderer>().enabled = false;
		isGameOver = true;
		rb.velocity = new Vector2(0,0);
		cam.gameObject.GetComponent<CameraBehaviour>().gameOver = true;
		GameObject planeTemp = Instantiate(brokenPlane, transform.position, transform.rotation);
		for(int i =0; i < planeTemp.transform.childCount; i++){
			Rigidbody2D rbTemp = planeTemp.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
			rbTemp.AddForce(((Vector2) missile.position - rbTemp.position) * -5f,ForceMode2D.Impulse);
		}
		Destroy(planeTemp, 6f);
		StartCoroutine(canvasStuff());
	}

	IEnumerator canvasStuff(){
		yield return new WaitForSeconds(1f);
		canvas.SetActive(true);
		GameObject.Find("Game").GetComponent<Game_Handle>().carrot.game.set_list_button_gamepad_console(GameObject.Find("Game").GetComponent<Game_Handle>().list_gamepad_gameover);
		for (int i = 0; i <= 10; i++){
			float k = (float) i /10;
			canvas.transform.localScale = new Vector3(k,k,k);
			yield return new WaitForSeconds(.01f);
		}
		Destroy(this.gameObject);
	}


	public void go_left()
    {
		this.is_move = true;
		this.timer_reset_move = 0;
		this.rotatePlane(-1);
	}

	public void go_right()
    {
		this.is_move = true;
		this.timer_reset_move = 0;
		this.rotatePlane(1);
	}
}
