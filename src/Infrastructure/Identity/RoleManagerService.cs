using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class RoleManagerService : IRoleManager
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleManagerService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<Result> CreateRole(AppRole role) =>
            (await _roleManager.CreateAsync(role)).ToApplicationResult();

        public Task<bool> AnyRoles() =>
            _roleManager.Roles.AnyAsync();
    }
}
