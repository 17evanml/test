using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour {
    bool Animating = false;
    bool inCombat = false;
    const float FLOORHEIGHT = -0.6f;
    const bool WIN = true,
                LOSE = false;
    public CombatQueue attackQueue = SingletonsCreator.AttackQueue();
    public CombatQueue defenseQueue = SingletonsCreator.DefenseQueue();
    public AnimationQueue animQueue = SingletonsCreator.AnimationQueue();
    public List<EnemyLogic> enemyLogics = new List<EnemyLogic>();
    public GameObject player;
    public PlayerController playerController;


    private void Start() {
        player = GameObject.Find("Player");
        playerController = (PlayerController)player.GetComponent("PlayerController");
    }

    void Update() {

    }

    public void AddEnemy(EnemyLogic enemy) {
        enemyLogics.Add(enemy);
    }
    public void RemoveEnemy(EnemyLogic enemy) {
        enemyLogics.Remove(enemy);
    }

    public void EnterCombat() {
        playerController.EnterCombat();
        if (!inCombat) {
            inCombat = true;
            StartCoroutine(RunCombat());
        }
    }

    public IEnumerator RunCombat() {
        while (attackQueue.Length() > 0) {
            CombatResponse attack = attackQueue.Next();
            Debug.DrawLine(attack.Self.transform.position, player.transform.position);
            switch (attack.AttackType) {
                case "Slash":
                    AddIndicator(attack.Self, Color.red);
                    break;
                case "Parry":
                    AddIndicator(attack.Self, Color.yellow);
                    break;
                case "Feint":
                    AddIndicator(attack.Self, Color.blue);
                    break;
            }
            yield return new WaitForSeconds(1);
            FillAnimQueue(attack);
        }
        if (!Animating) {
            StartCoroutine(ExecuteCombat());
        }
    }

    public IEnumerator ExecuteCombat() {
        defenseQueue.Clear();
        Animating = true;
        List<Animatable> deadOnes = new List<Animatable>();
        for (int i = 0; i < enemyLogics.Count; i++) {
            enemyLogics[i].EnterCombat();
        }
        while (animQueue.Length() > 0) {
            yield return new WaitForSeconds(1);
            AnimationBean currentSet = animQueue.Next();
            if (currentSet.Win == WIN) {
                deadOnes.Add(currentSet.animatable);
            }
            else {
                deadOnes.Add(playerController);
            }
            RunAnimation(currentSet);
        }
        yield return new WaitForSeconds(1.5f);
        AnimateDeaths(deadOnes);
        yield return new WaitForSeconds(1);
        ExitCombat();
    }

    private void ExitCombat() {
        playerController.ExitCombat();
        for (int i = 0; i < enemyLogics.Count; i++) {
            enemyLogics[i].ExitCombat();
        }
        inCombat = false;
        Animating = false;

    }
    private void RunAnimation(AnimationBean anim) {
        //Get a unit vector pointing from the anim to the player.
        Vector3 playerPos = player.transform.position;
        Vector3 animPos = anim.animatable.transform.position;
        Vector3 dirVec = playerPos - animPos;
        dirVec /= dirVec.magnitude;

        //remove the indicator
        Destroy(anim.animatable.gameObject.GetComponent<LineRenderer>());
        if (anim.CharAttack == "Parry") {
            //anim.SetTrigger("isParry");
            //run parry animation here

        }
        else if (anim.CharAttack == "Feint") {

            Vector3 tempPos = CheckAboveFloor(animPos + dirVec);
            StartCoroutine(MoveAtoB(tempPos, player, 100f));
            //player.transform.position = tempPos;
        }
        else if (anim.CharAttack == "Slash") {
            Vector3 tempPos = CheckAboveFloor(animPos - dirVec);
            StartCoroutine(MoveAtoB(tempPos, player, 100f));
        }
        if (anim.EnemyAttack == "Parry") {

        }
        else if (anim.EnemyAttack == "Feint") {
            Vector3 tempPos = CheckAboveFloor(playerPos - dirVec);
            StartCoroutine(MoveAtoB(tempPos, anim.animatable.gameObject, 100f));
        }
        else if (anim.EnemyAttack == "Slash")
        {
            Vector3 tempPos = CheckAboveFloor(playerPos + dirVec);
            StartCoroutine(MoveAtoB(tempPos, anim.animatable.gameObject, 100f));
        }
    }
    private void AnimateDeaths(List<Animatable> deaths) {
        for (int i = 0; i < deaths.Count; i++) {
            deaths[i].Death();
        }
    }

    public void AddIndicator(GameObject enemy, Color indicatorColor) {
        enemy.AddComponent<LineRenderer>();
        LineRenderer lineRend = enemy.GetComponent<LineRenderer>();
        Vector3[] toPlayer = { player.transform.position, enemy.transform.position };
        lineRend.SetPositions(toPlayer);
        lineRend.endWidth /= 2;
        lineRend.startWidth = 0;
        Material particleMat = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lineRend.material = particleMat;
        lineRend.startColor = indicatorColor;
        lineRend.endColor = indicatorColor;
    }

    public void FillAnimQueue(CombatResponse attack) {
        if (defenseQueue.Length() > 0)
        {
            CombatResponse defense = defenseQueue.Next();
            bool victor = attack.Battle(defense);
            if (victor == WIN) {
                animQueue.Add(new AnimationBean(attack.AttackType, defense.AttackType, WIN, attack.Self.GetComponent<EnemyLogic>()));
            }
            else {
                animQueue.Add(new AnimationBean(attack.AttackType, defense.AttackType, LOSE, attack.Self.GetComponent<EnemyLogic>()));
            }
        }
        else {
            animQueue.Add(new AnimationBean(attack.AttackType, "nothing", LOSE, attack.Self.GetComponent<EnemyLogic>()));
        }
    }
    public Vector3 CheckAboveFloor(Vector3 position) {
        if (position.y < FLOORHEIGHT)
        {
            position.y = FLOORHEIGHT;
        }
        return position;
    }

    private IEnumerator MoveAtoB(Vector3 end, GameObject item, float speed) {
       
        Vector3 startPos = item.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPos, end, Mathf.SmoothStep(0f, speed, t));
            yield return null;
        }

    }
}