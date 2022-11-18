using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using server.Entities;

namespace server.Authorization;

public class EntryResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Entry>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Entry entry)
	{
		if(requirement.ResourceOperation == ResourceOperation.Create)
		{
			context.Succeed(requirement);
		}

		if (requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Update || requirement.ResourceOperation == ResourceOperation.Delete)
		{
			var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

			if (entry.UserId == int.Parse(userId))
			{
				context.Succeed(requirement);
			}
		}
		
		return Task.CompletedTask;
	}
}