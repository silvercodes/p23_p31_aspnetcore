using Microsoft.AspNetCore.Authorization;

namespace _05_custom_requirements.Requirements;

public class BusinessHoursHandler : AuthorizationHandler<BusinessHoursRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        BusinessHoursRequirement requirement)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);

        if (now >= requirement.StartTime && now <= requirement.EndTime)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
