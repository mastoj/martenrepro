namespace MartenRepro.Api.Modules.TemplateFolder;

using System.Threading.Tasks;
using Carter;
using MartenRepro.Core.Domain.TemplateFolder;
using MartenRepro.Core.Infrastructure;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

public record CreateTemplateFolderRequest(Guid id, string name);
public record AddGroupRequest(Guid id, string groupName);

public enum ErrorCodes
{
    DuplicateId = 1001,
    DuplicateFolderName = 1002,
    GroupAlreadyExists = 1003
}

public class TemplateFolderModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/templatefolder", CreateTemplateFolder)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        app.MapPost("/templatefolder/{folderId}/groups", AddGroup);
    }

    private async Task<object> CreateTemplateFolder(Application application, [FromBody] CreateTemplateFolderRequest request)
    {
        try
        {
            var result = await application.Execute(new CreateTemplateFolder(request.id, request.name));
            return Results.Ok(result);
        }
        catch (DuplicateIdException)
        {
            return Results.BadRequest(new { message = "Folder already exists for that id", errorCode = ErrorCodes.DuplicateId});
        }
        catch (DuplicateFolderException)
        {
            return Results.BadRequest(new { message = "Folder already exists for that name", errorCode = ErrorCodes.DuplicateFolderName});
        }
    }

    public object AddGroup(Application application, Guid folderId, AddGroupRequest request)
    {   
        try
        {
            var result = application.Execute(new AddGroup(folderId, request.groupName));
            return Results.Ok(result);
        }
        catch (GroupIsAlreadyAddedException)
        {
            return Results.BadRequest(new { message = "Group already exists", errorCode = ErrorCodes.GroupAlreadyExists });
        }
    }
}
