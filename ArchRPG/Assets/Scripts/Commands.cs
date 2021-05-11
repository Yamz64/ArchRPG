using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase
{
    private string _id;
    private string _description;
    private string _format;

    public string GetID() { return _id; }
    public string GetDescription() { return _description; }
    public string GetFormat() { return _format; }

    public CommandBase(string id, string description, string format)
    {
        _id = id;
        _description = description;
        _format = format;
    }
}

public class Command : CommandBase
{
    private Action command;

    public Command(string id, string description, string format, Action command) : base (id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class Command<T> : CommandBase
{
    private Action<T> command;

    public Command(string id, string description, string format, Action<T> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T value)
    {
        command.Invoke(value);
    }
}
