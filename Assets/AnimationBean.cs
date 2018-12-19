using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBean {
    public string EnemyAttack { get; private set; 
    }
    public string CharAttack { get; private set; }
    public int Win { get; private set; }
    public Animatable Enemy { get; private set; }
    public Animatable This { get; private set; }
    public AnimationBean (string enemyAttackInit, string charAttackInit, int winInit, Animatable enemyInit) {
        EnemyAttack = enemyAttackInit;
        CharAttack = charAttackInit;
        Win = winInit;
        Enemy = enemyInit;
    }
}
