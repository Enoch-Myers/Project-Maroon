using UnityEngine;

//First stage of boss
public class BStage1 : BaseBoss
{
    public override void TakeDamage(int damageAmount){
        //Normal Damage
        base.TakeDamage(damageAmount);
        
        if(currentHealth <= 0){
            //Start stage 2
            public gameObject BStage2;
            if(BStage2 != null){
                Instantiate(BStage2, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
    public override void Update(){
        base.Update();
        //Additional Logic Later
    }
}
