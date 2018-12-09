using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatResponse {
    public static int WIN = 1,
                        LOSE = -1,
                        TIE = 0;

    public int HorizDir { get; private set; }
    public int VertDir { get; private set; }
    public string AttackType { get; private set; }
    public GameObject Self { get; private set; }
    public CombatResponse(int horizDirInit, int vertDirInit, string attackTypeInit, GameObject selfInit) {
        HorizDir = horizDirInit;
        VertDir = vertDirInit;
        AttackType = attackTypeInit;
        Self = selfInit;
    }

    public int Battle(CombatResponse defender) {
        if (DirCheck(this.HorizDir, defender.HorizDir) && DirCheck(this.VertDir, defender.VertDir)) {
            return SingletonsCreator.SingleMoveSetTable().Compare(this.AttackType, defender.AttackType);
        } else {
            return -1;
        }
    }

    private bool DirCheck(int attacker, int defender) {
        if(attacker == 0 && defender == 0) {
            return true; //win
        } else if (defender == -attacker) {
            return true; //win
        } else {
            return false; //false
        }
    }
}
