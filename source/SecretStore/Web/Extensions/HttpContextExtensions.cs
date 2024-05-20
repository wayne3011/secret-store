namespace SecretStore.Web.Controllers.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        return (Guid)context.Items["UserId"];
    }
}