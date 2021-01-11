using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum battleState {  START, PLAYER, ENEMY, WIN, LOSE }

public class BattleScript : MonoBehaviour
{
    /*
    Steps:

    Access party member data
    For each member:
	    Add name into the order
	    Define character as member (have actions/stats ready)
		    Could also grab player data directly and load character object in
	    Get sprite ready

    Get enemy data for stage (stage data as well)
    For each enemy:
	    Add name into the order
	    Define character as enemy (record stats and possible actions)
	    Get AI ready
	    Get Sprite ready

    For each member/enemy in the list:
	    Player:
		    Choose action (attack, defend, use item, etc.)
	    Enemy:
		    Have action chosen based on probability/circumstances (AI)

	    Record damage from attacks, skip characters who have health below 0

    Complementary Steps:
	    Reorder characters if a speed stat has been changed/ability used
	    Skip over characters whose health is < 0
     */

    public battleState state;

    public Text dialogue;

    public GameObject playerPrefab;
    public GameObject member1Prefab;
    public GameObject member2Prefab;
    public GameObject member3Prefab;
    public GameObject enemyPrefab;

    public Transform playerStation;
    public Transform member1Station;
    public Transform member2Station;
    public Transform member3Station;
    public Transform enemyStation;

    unit playerUnit;
    unit member1Unit;
    unit member2Unit;
    unit member3Unit;
    unit enemyUnit;

    void Start()
    {
        state = battleState.START;
        setupBattle();
    }

    void setupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGo.GetComponent<unit>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGo.GetComponent<unit>();

        if (member1Prefab && member1Station)
        {
            GameObject member1Go = Instantiate(member1Prefab, member1Station);
            member1Unit = member1Go.GetComponent<unit>();
        }

        if (member2Prefab && member2Station)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Unit = member2Go.GetComponent<unit>();
        }

        if (member3Prefab && member3Station)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Unit = member3Go.GetComponent<unit>();
        }

        dialogue.text = "The " + enemyUnit.unitName + " appears.";
    }


}
