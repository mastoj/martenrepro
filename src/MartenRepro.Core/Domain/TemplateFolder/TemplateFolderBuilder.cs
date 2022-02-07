using MartenRepro.Core.Infrastructure;

using Marten.Events.Aggregation;

namespace MartenRepro.Core.Domain.TemplateFolder;

public class TemplateFolderBuilder : AggregateProjection<TemplateFolder>//, IProjection<TemplateFolder, TemplateFolderEvent>
{

    public TemplateFolder Apply(TemplateFolderEvent @event, TemplateFolder document)
    {
        return @event switch
        {
            TemplateFolderCreated templateFolderCreated => ApplyEvent(templateFolderCreated, document),
            GroupAdded groupAdded => ApplyEvent(groupAdded, document),
            _ => throw new NotImplementedException()
        };
    }

    private TemplateFolder ApplyEvent(TemplateFolderCreated templateFolderCreated, TemplateFolder templateFolder)
    {
        return templateFolder with
        {
            Id = templateFolderCreated.Id,
            Name = templateFolderCreated.Name
        };
    }

    private TemplateFolder ApplyEvent(GroupAdded evt, TemplateFolder state)
    {
        Console.WriteLine("==> Adding group to template folder");
        var groups = state.Groups.Append(evt.Group);
        return state with { Groups = groups };
    }}

