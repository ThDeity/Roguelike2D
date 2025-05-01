using UnityEngine;

public class FireRoll : MonoBehaviour, Roll
{
    public TrailOfFire trail;

    Vector2 _startPos, _endPos;
    public virtual void OnRollStarted() => _startPos = transform.position;

    public virtual void OnRollFinished()
    {
        _endPos = transform.position;
        Transform t = Instantiate(trail.gameObject, _startPos, Quaternion.identity).transform;
        float distance = Vector2.Distance(_startPos, _endPos);

        t.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.position.y - t.position.y, transform.position.x - t.position.x) * Mathf.Rad2Deg - 90);
        t.localScale = new Vector2(t.localScale.x, t.localScale.y * distance);

        t.tag = tag;
        t.gameObject.layer = gameObject.layer;
    }
}
