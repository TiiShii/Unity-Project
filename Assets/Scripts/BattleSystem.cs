﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    public class BattleSystem : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject enemyPrefab;

        public Transform playerBattleStation;
        public Transform enemyBattleStation;

        Unit playerUnit;
        Unit enemyUnit;

        public Text dialogueText;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        public BattleState state;

        void Start()
        {
            state = BattleState.START;
            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
            playerUnit = playerGO.GetComponent<Unit>();

            GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit>();

            dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        IEnumerator PlayerSpell(Button button)
        {
            int spellIndex = int.Parse(button.name);

            bool isDead = enemyUnit.TakeDamage(playerUnit.spells[spellIndex].damage);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "The spell " + playerUnit.spells[spellIndex].spellName + " is cast successfully!";

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }

        IEnumerator EnemyTurn()
        {
            dialogueText.text = enemyUnit.unitName + " attacks!";

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

        void EndBattle()
        {
            if (state == BattleState.WON)
            {
                dialogueText.text = "You won the battle!";
            }
            else if (state == BattleState.LOST)
            {
                dialogueText.text = "You were defeated.";
            }
        }

        void PlayerTurn()
        {
            dialogueText.text = "Choose a spell:";
        }

        public void OnSpellButton(Button button)
        {
            if (state != BattleState.PLAYERTURN)
                return;

            StartCoroutine(PlayerSpell(button));
        }
}
