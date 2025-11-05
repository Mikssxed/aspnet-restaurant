using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization;

public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    private readonly RestaurantDbContext _context;

    public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        if (_context == null)
            return Task.CompletedTask;

        var nameIdentifierClaim = context.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null || string.IsNullOrEmpty(nameIdentifierClaim.Value))
            return Task.CompletedTask;

        if (!int.TryParse(nameIdentifierClaim.Value, out var userId))
            return Task.CompletedTask;

        var createdRestaurantsCount = _context.Restaurants?.Count(r => r.CreatedById == userId) ?? 0;

        if (createdRestaurantsCount >= requirement.MinimumRestaurantsCreated) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}