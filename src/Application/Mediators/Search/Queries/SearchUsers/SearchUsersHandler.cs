using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Search.Queries.SearchUsers
{
    public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserVm>>
    {
        private readonly IUserManager _userManager;

        public SearchUsersHandler(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IEnumerable<UserVm>> Handle(SearchUsersQuery request, CancellationToken cancellationToken) =>
            string.IsNullOrWhiteSpace(request.Term)
                ? Enumerable.Empty<UserVm>()
                : await _userManager.SearchUsers(request.Term);
    }
}