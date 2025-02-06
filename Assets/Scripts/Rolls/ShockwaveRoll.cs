using UnityEngine;

public class ShockwaveRoll : Explosion, Roll
{
    public virtual void OnRollFinished() => Shockwave();

    protected override void OnTriggerEnter2D(Collider2D collision) {}

    protected override void OnCollisionEnter2D(Collision2D collision) { }
}
