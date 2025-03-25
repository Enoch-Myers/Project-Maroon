using UnityEngine;

//Second stage of boss
public class BStage2 : BaseBoss
{
    //Half damage
    public override void TakeDamage(int damageAmount){
        int actualDamage = Mathf.FloorToInt(damageAmount * 0.5f);
        base.TakeDamage(actualDamage);
    }

    public override void Update(){
        base.Update();
        //Additional Logic Later
    }
}