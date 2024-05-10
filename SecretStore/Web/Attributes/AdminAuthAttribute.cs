using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecretStore.Web.Options;

namespace SecretStore.Web.Attributes;

public class AdminAuthAttribute : Attribute
{
}