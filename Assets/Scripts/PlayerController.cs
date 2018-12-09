using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Animatable {
    public float dirX, moveSpeed = 2f;
    public float runSpeed = 5f;
    private bool isHurting, isDead, isParry, isUnsheathe, up, down, left, right;
    private bool facingLeft = true;

    private Animator anim;
    private CombatHandler combatHandler;

    Vector3 localScale;

    //private int speed = 10;
    //private bool up, down, left, right;
    //private CombatHandler combatHandler;



    // Use this for initialization
    new void Start() {
        base.Start();
        combatHandler = GameObject.Find("GameRules")
                                  .GetComponent<CombatHandler>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        KeysDown();
        KeysUp();
        if (!base.inCombat) {
            animatedMovement();
        }
        else {
            ChooseAttack();
        }
    }

    void FixedUpdate() {
        if (!isHurting)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate() {
        CheckWhereToFace();
    }

    void KeysDown() {
        if (Input.GetKeyDown("up")) {
            up = true;
        }
        if (Input.GetKeyDown("down")) {
            down = true;
        }
        if (Input.GetKeyDown("left")) {
            left = true;
        }
        if (Input.GetKeyDown("right")) {
            right = true;
        }
    }

    void KeysUp() {
        if (Input.GetKeyUp("up")) {
            up = false;
        }
        if (Input.GetKeyUp("down")) {
            down = false;
        }
        if (Input.GetKeyUp("left")) {
            left = false;
        }
        if (Input.GetKeyUp("right")) {
            right = false;
        }
    }

    CombatResponse PostAttack(string attack) {
        int vertDir = 0;
        int horizDir = 0;
        if (left) {
            horizDir = -1;
            //anim.SetTrigger("isSheathe");
        }
        else if (right) {
            horizDir = 1;
            //anim.SetTrigger("isSheathe");
        }
        if (up) {
            vertDir = 1;
            //anim.SetTrigger("isSheathe");
        }
        else if (down) {
            vertDir = -1;
            //anim.SetTrigger("isSheathe");
        }
        //Debug.Log("Player: " + vertDir + " | " + horizDir);
        return new CombatResponse(horizDir, vertDir, attack, gameObject);
    }

    void ChooseAttack() {
        if (Input.GetKeyDown("z")) {
            combatHandler.defenseQueue.Add(PostAttack("Parry"));
            //anim.SetTrigger("isUnsheathe");
        }
        else if (Input.GetKeyDown("x")) {
            combatHandler.defenseQueue.Add(PostAttack("Slash"));
            //anim.SetTrigger("isUnsheathe");
        }
        else if (Input.GetKeyDown("c")) {
            combatHandler.defenseQueue.Add(PostAttack("Feint"));
            //anim.SetTrigger("isUnsheathe");
        }
    }
    override public void Death() {
        dirX = 0;
        isDead = true;
        anim.SetTrigger("isDead");
    }

    public override void ExitCombat() {
        print("instant");
        base.ExitCombat();
    }


    void SetAnimationState() {
        if (dirX == 0) {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }

        if (rb.velocity.y == 0) {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (Mathf.Abs(dirX) == 2 && rb.velocity.y == 0) {
            anim.SetBool("isWalking", true);
        }

        if (Mathf.Abs(dirX) == 5 && rb.velocity.y == 0) {
            anim.SetBool("isRunning", true);
        }
        else {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Mathf.Abs(dirX) == 5) {
            anim.SetBool("isSliding", true);
        }
        else {
            anim.SetBool("isSliding", false);
            //Circle2D.enabled = !Circle2D.enabled;
        }

        if (rb.velocity.y > 0) {
            anim.SetBool("isJumping", true);
        }

        if (rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }

        //if (Input.GetKeyDown("z"))
        //{
        //    anim.SetTrigger("isParry");
        //}

        //if (Input.GetButtonDown("z"))
        //    anim.SetBool("isSlapping", true);
        //else
        //anim.SetBool("isSlapping", false);
    }

    void CheckWhereToFace() {
        if (dirX < 0) {
            facingLeft = true;
        }
        else if (dirX > 0) {
            facingLeft = false;
        }

        if (((facingLeft) && (localScale.x < 0)) || ((!facingLeft) && (localScale.x > 0))) {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }

    void animatedMovement() {
        if (Input.GetButtonDown("Jump") && (!isDead) && rb.velocity.y == 0)
            rb.AddForce(Vector2.up * 400f);

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = runSpeed;
        else
            moveSpeed = 2f;

        SetAnimationState();

        if (!isDead)
            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (isParry)
            dirX = 0;
    }
}




//Archived
//IEnumerator Hurt()
//{
//isHurting = true;
//rb.velocity = Vector2.zero;

//if (facingRight)
//    rb.AddForce(new Vector2(200f, 200f));
//else
//    rb.AddForce(new Vector2(-200f, 200f));

//yield return new WaitForSeconds(0.5f);

//isHurting = false;


//void FreeMovement()
//{
//    if (up) {
//        base.rb.AddForce(transform.up * speed);
//    }
//    if (left)
//    {
//        base.rb.AddForce(-transform.right * speed);
//    }
//    if (right)
//    {
//        base.rb.AddForce(transform.right * speed);
//    }
//}

//void OnTriggerEnter2D(Collider2D col)
//{

//    if (col.gameObject.CompareTag("Enemy") && healthPoints > 0)
//    {
//        dirX = 0;
//        isDead = true;
//        anim.SetTrigger("isDead");
//    }
//} CHECK THIS OUT