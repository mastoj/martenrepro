namespace MartenRepro.Core.Domain.TemplateFolder;

using System;
using System.Collections.Generic;

using MartenRepro.Core.Infrastructure;

public static class Decider
{
    public static IEnumerable<TemplateFolderEvent> Handle(CreateTemplateFolder command, TemplateFolder? state, Func<string, bool> folderExists)
    {
        if (state != null && command.Id == state.Id)
        {
            throw new DuplicateIdException();
        }

        if (folderExists(command.Name))
        {
            throw new DuplicateFolderException(command.Name);
        }

        return new List<TemplateFolderEvent> {
            new TemplateFolderCreated(command.Id, command.Name)
        };
    }

    public static IEnumerable<TemplateFolderEvent> Handle(AddGroup command, TemplateFolder state)
    {
        Console.WriteLine("==> Adding group to template folder");
        var group = new Group(command.Name);
        if(state.Groups.Contains(group)) {
            throw new GroupIsAlreadyAddedException(command.Name);
        }
        return new List<TemplateFolderEvent> {
            new GroupAdded(command.Id, group)
        };
    }

    public static Func<TemplateFolderCommand, TemplateFolder, IEnumerable<TemplateFolderEvent>> Create(Func<string, bool> folderExists)
    {
        return (command, state) =>
        {
            switch (command)
            {
                case CreateTemplateFolder cmd:
                    return Handle(cmd, state, folderExists);
                case AddGroup cmd:
                    return Handle(cmd, state);
                default:
                    throw new NotImplementedException($"Invalid command {command.GetType().FullName}");
            };
        };
    }
}
