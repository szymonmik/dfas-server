﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using server.Entities;

namespace server.Authorization;

public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, User>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
		ResourceOperationRequirement requirement, User user)
	{
		if(requirement.ResourceOperation == ResourceOperation.Read 
		   || requirement.ResourceOperation == ResourceOperation.Create)
		{
			context.Succeed(requirement);
		}

		var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

		if(user.Id == int.Parse(userId))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}

