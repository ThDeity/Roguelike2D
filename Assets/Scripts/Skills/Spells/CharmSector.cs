using System.Collections.Generic;
using UnityEngine;

public class CharmSector : MonoBehaviour
{
    public List<DebuffsEffects> effects = new List<DebuffsEffects>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DebuffsEffects effect))
            effects.Add(effect);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DebuffsEffects effect) && effects.Contains(effect))
            effects.Remove(effect);
    }
}
