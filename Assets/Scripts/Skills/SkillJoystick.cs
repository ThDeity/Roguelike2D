using UnityEngine.EventSystems;

public class SkillJoystick : Joystick
{
    public bool isDragging, isDowned;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        isDragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        isDragging = false;
    }
}
