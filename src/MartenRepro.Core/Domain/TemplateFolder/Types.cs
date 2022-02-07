namespace MartenRepro.Core.Domain.TemplateFolder;

using System;

using MartenRepro.Core.Infrastructure;

#region Common
public record Group(string Name);
#endregion

#region Commands
public abstract record TemplateFolderCommand(Guid Id) : Command(Id);

public record CreateTemplateFolder(Guid Id, string Name) : TemplateFolderCommand(Id);

public record AddGroup(Guid Id, string Name) : TemplateFolderCommand(Id);
#endregion

#region Events
public abstract record TemplateFolderEvent(Guid Id) : Event(Id);

public record TemplateFolderCreated(Guid Id, string Name) : TemplateFolderEvent(Id);

public record GroupAdded(Guid Id, Group Group) : TemplateFolderEvent(Id);

#endregion

public record TemplateFolder(Guid Id, string Name, IEnumerable<Group> Groups)
{
    // public static TemplateFolder Initial = new(Guid.Empty, string.Empty, Enumerable.Empty<Group>());
    public TemplateFolder() : this(Guid.Empty, string.Empty, Enumerable.Empty<Group>())
    { }
}

#region Exceptions
public class DuplicateFolderException : Exception
{
    public DuplicateFolderException(string? message) : base(message)
    {
    }
}

public class GroupIsAlreadyAddedException : Exception
{
    public GroupIsAlreadyAddedException(string? message) : base(message)
    {
    }
}

#endregion
