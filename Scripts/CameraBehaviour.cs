using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBehaviour : MonoBehaviour {

	public Transform target;
	public Transform arean_missile;
	public Transform arean_missile_dust;
	public GameObject obj_tip_help;
	public GameObject missilePrefab;
	public float speed = 5f;
	public bool gameOver=true;
	Vector3 offSet;

	public void  load() {
		offSet = target.position - transform.position;
	}

	void LateUpdate () {
		if(!gameOver) transform.position = Vector3.Lerp(transform.position, target.position - offSet, speed * Time.deltaTime);
	}

	IEnumerator missileSpawner(){
		while(!gameOver){
			int j, i;
			if(target.rotation.z < 180) {
				i = 10;
				j = 8;
			}
			else {
				i = -10;   
				j = -8;
			}
		Vector3 spawnPosition = target.position + new Vector3(Random.Range(j,i),Random.Range(j,i),0f);
		GameObject missileTemp = Instantiate(missilePrefab, spawnPosition, missilePrefab.transform.rotation);
		missileTemp.transform.SetParent(this.arean_missile);
			missileTemp.GetComponent<MissileBehaviour>().arean_dust = this.arean_missile_dust;
		missileTemp.GetComponent<MissileBehaviour>().target = target;
		yield return new WaitForSeconds(Random.Range(3f,5f));
		}
	}

	public void show_effect_tip()
    {
		this.obj_tip_help.SetActive(true);
		StartCoroutine(startAnimation_effect_tip());
	}

	IEnumerator startAnimation_effect_tip(){
		yield return new WaitForSeconds(2f);
		this.obj_tip_help.SetActive(false);
	}

	public void load_missileSpawner()
    {
		StartCoroutine(missileSpawner());
	}

}
