
using System.Collections.Generic;

public enum MoveType
{
    High,
    Middle,
    Low
}

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right
}

public class NoteForJudge
{
    public float Time;
    public MoveType Type;
    public List<DirectionType> Directions;
}