using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApp.Extentions
{
    public class AdminRequirement : IAuthorizationRequirement
    {
    }

    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var userType = context.User.FindFirst(c => c.Type == ClaimTypes.AuthorizationDecision);

            if (userType is null)
            {
                return Task.CompletedTask;
            }

            if (userType.Value == UserType.Admin.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }



    public class StaffRequirement : IAuthorizationRequirement
    {

    }

    public class StaffRequirementHandler : AuthorizationHandler<StaffRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StaffRequirement requirement)
        {
            var userType = context.User.FindFirst(c => c.Type == ClaimTypes.AuthorizationDecision);

            if (userType is null)
            {
                return Task.CompletedTask;
            }

            if (userType.Value == UserType.Staff.ToString() || userType.Value == UserType.Admin.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}