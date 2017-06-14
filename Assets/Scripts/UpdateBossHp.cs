using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBossHp : MonoBehaviour {
	int Hp = 100;
	GameObject boss;
    Text btn;
	void Start(){
		btn = GetComponent<Text> ();
		boss = GameObject.Find ("Enemy");
	}
	public void updateHP(){
		if (Hp > 0) {
			Hp -= 10;
			btn.text = "Boss HP " + Hp + "/100";
		}
		if(Hp <=0){
			boss.SetActive (false);
            btn.text = "Boss dead";
            
        }
	}
	public void resetBoss(){
		Hp = 100;
		boss.SetActive (true);
		btn.text = "Boss HP " + Hp + "/100";

	}

		
}
