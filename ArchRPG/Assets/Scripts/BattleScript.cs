using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
