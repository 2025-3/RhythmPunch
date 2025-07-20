
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerateCounterListsStrategy
{
    public abstract List<List<DirectionType>> Generate();

    protected List<DirectionType> GenerateCommand(int length)
    {
        List<DirectionType> command = new();
        for (int i = 0; i < length; i++)
        {
            command.Add((DirectionType)Random.Range(0, 4));
            if (i >= 1 &&
                ((command[i - 1] == DirectionType.Up && command[i] == DirectionType.Up) ||
                 (command[i - 1] == DirectionType.Down && command[i] == DirectionType.Down)))
            {
                command.Clear();
                i = -1;
            }
        }

        return command;
    }
}

public class Stage0 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new();
        return commands;
    }
}

public class Stage1 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new() { GenerateCommand(1) };
        return commands;
    }
}

public class Stage2 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new() { GenerateCommand(1), GenerateCommand(2) };
        return commands;
    }
}

public class Stage3 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new() { GenerateCommand(2), GenerateCommand(2) };
        return commands;
    }
}

public class Stage4 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new() { GenerateCommand(3), GenerateCommand(2) };
        return commands;
    }
}

public class Stage5 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new()
            {
                GenerateCommand(3),
                GenerateCommand(3),
                GenerateCommand(2)
            };
        return commands;
    }
}

public class Stage6 : GenerateCounterListsStrategy
{
    public override List<List<DirectionType>> Generate()
    {
        List<List<DirectionType>> commands = new()
        {
            GenerateCommand(4),
            GenerateCommand(3),
            GenerateCommand(2)
        };
        return commands;
    }
}