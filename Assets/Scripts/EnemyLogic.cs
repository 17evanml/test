using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : Animatable {
    public int range = 5;
    private double DiagThreshold = 1.5;

    public GameObject player;
    private CombatHandler combatHandler;
    float dirX, moveSpeed = 2f;
    bool isHurting, isDead, isKicking, isTesting;
    bool facingRight = true;
    Vector3 localScale;
    //Animator anim;

    private int[] AngleRanges = { 20, 72, 108, 160 };

    // Use this for initialization
    new void Start () {
        base.Start();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        combatHandler = GameObject.Find("GameRules").GetComponent<CombatHandler>();
        combatHandler.AddEnemy(this);
        localScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < AngleRanges.Length; i++){
            Vector3 thisLine = Quaternion.Euler(0, 0, AngleRanges[i]) * transform.right*10;
            Debug.DrawRay(player.transform.position, thisLine);
            thisLine = Quaternion.Euler(0, 0, 360-AngleRanges[i]) * transform.right * 10;
            Debug.DrawRay(player.transform.position, thisLine);
        }
        WatchPlayer();
	}

    override public void Death() {
        combatHandler.RemoveEnemy(this);
        anim.SetTrigger("isDead");
        StartCoroutine(TrueDeath(2));
    }

    void WatchPlayer() {
        if (!inCombat) {
            anim.SetBool("isRunning", true);
            Vector3 distVec = transform.position - player.transform.position;


            if (distVec.x > 0)
                facingRight = true;
            else if (distVec.x < 0)
                facingRight = false;

            if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
                localScale.x *= -1;

            transform.localScale = localScale;


            if (distVec.magnitude < range) {
                EnterCombat();
                anim.SetBool("isRunning", false);
                Debug.Log("Attack Added! " + this);
                combatHandler.attackQueue.Add(ChooseAttack());
                combatHandler.EnterCombat();
            }



            distVec /= distVec.magnitude;
            rb.AddForce(distVec*-4);
        }
    }



    override public void ExitCombat() {
        Debug.Log("exited Combat");
        base.ExitCombat();
    }

    CombatResponse ChooseAttack() {
        int vertDir = 0;
        int horizDir = 0;
        Vector3 refPos = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.right, refPos);

        if(angle < 22.5) {
            horizDir = 1;
        } else if (angle > AngleRanges[0] && angle < AngleRanges[1]) {
            horizDir = 1;
            vertDir = CheckAbove();
        } else if (angle > AngleRanges[1] && angle < AngleRanges[2]) {
            vertDir = CheckAbove();
        } else if (angle > AngleRanges[2] && angle < AngleRanges[3])
        {
            horizDir = -1;
            vertDir = CheckAbove();
        } else {
            horizDir = -1;
        }

        Debug.Log("Enemy: " + vertDir + "|" + horizDir);
        int attack = Random.Range(0, 3);

        if (attack == 0) {
            print("SLASH");
            anim.SetTrigger("isSheathe");
            return new CombatResponse(horizDir, vertDir, "Slash", gameObject);
        } else if (attack == 1) {
            print("FEINT");
            anim.SetTrigger("isSheathe");
            return new CombatResponse(horizDir, vertDir, "Feint", gameObject);
        } else {
            print("PARRY");
            anim.SetTrigger("isSheathe");
            return new CombatResponse(horizDir, vertDir, "Parry", gameObject);
        }
    }
    int CheckAbove() {
        return player.transform.position.y - transform.position.y < 0 ? -1 : 1;
    }

}
