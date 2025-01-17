﻿using System.Reflection;
using System.Security.Claims;
using ExpediFlow.Constants;
using ExpediFlow.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpediFlow.Helpers
{
    public static class ClaimsHelper
    {
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == role.Name && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(role.Name, permission));
            }
        }

        public static async Task RemovePermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var claimToRemove = allClaims.FirstOrDefault(a => a.Type == role.Name && a.Value == permission);
            if (claimToRemove != null)
            {
                await roleManager.RemoveClaimAsync(role, claimToRemove);
            }
        }


    }


}
