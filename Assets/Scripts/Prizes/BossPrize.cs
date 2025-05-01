using UnityEngine;

public class BossPrize : Prize
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && !StaticValues.WasPrizeGotten)
        {
            StaticValues.PlayerObj.TakeDamage(-StaticValues.PlayerObj.ChangeMxHp(1), 0);

            StaticValues.WasPrizeGotten = true;
            Destroy(gameObject);
        }
    }
}
