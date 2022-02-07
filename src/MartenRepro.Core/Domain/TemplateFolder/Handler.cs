using MartenRepro.Core.Infrastructure;

namespace MartenRepro.Core.Domain.TemplateFolder;

public record HandlerBuilder : HandlerBuilder<TemplateFolderCommand, TemplateFolder, TemplateFolderEvent>
{
    public Func<string, bool> FolderExists { get; init; }

    public override Handler<TemplateFolderCommand, TemplateFolder, TemplateFolderEvent> Build()
    {
        return new Handler<TemplateFolderCommand, TemplateFolder, TemplateFolderEvent>(Decider.Create(FolderExists), (s, e) => new TemplateFolderBuilder().Apply(e, s), EventStore);
    }

    public HandlerBuilder With(Func<string, bool> folderExists)
    {
        return this with { FolderExists = folderExists };
    }

}
