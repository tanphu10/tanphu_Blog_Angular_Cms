namespace TPBlog.Api.Services.IServices
{
    public interface IPermissionService
    {
        Task<List<string>> UserHasPermissionForProjectAsync();
    }

}
