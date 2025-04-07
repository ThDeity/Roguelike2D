using UnityEngine;

public class BossPrize : Prize
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && !StaticValues.WasPrizeGotten)
        {
            StaticValues.WasPrizeGotten = true;
            StaticValues.PlayerObj.TakeDamage(-StaticValues.PlayerObj.ChangeMxHp(1), 0);

            Destroy(gameObject);
        }
    }
}
