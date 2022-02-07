namespace MartenRepro.Core.Infrastructure;

using System;

public abstract record Command(Guid Id);
public abstract record Event(Guid Id);
public interface IDocumentCreated
{
    public Guid Id { get; }
};

#region Exceptions
public class DuplicateIdException : Exception
{
}
#endregion
