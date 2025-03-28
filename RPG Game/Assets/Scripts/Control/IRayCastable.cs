using RPG.Control;

public interface IRayCastable
{
    CursorType GetCursorType();
    bool HandleRaycast(PlayerController callingController);
}
