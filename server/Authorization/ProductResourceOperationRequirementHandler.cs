using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using server.Entities;

namespace server.Authorization;

public class ProductResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Product>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Product product)
	{
		
		if(requirement.ResourceOperation == ResourceOperation.Create)
		{
			context.Succeed(requirement);
		}

		if (requirement.ResourceOperation == ResourceOperation.Read)
		{
			var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

			if (product.UserId == int.Parse(userId) || product.UserId is null)
			{
				context.Succeed(requirement);
			}
		}
		
		if (requirement.ResourceOperation == ResourceOperation.Update)
		{
			var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

			if (product.UserId == int.Parse(userId))
			{
				context.Succeed(requirement);
			}
		}

		

		return Task.CompletedTask;
	}
}