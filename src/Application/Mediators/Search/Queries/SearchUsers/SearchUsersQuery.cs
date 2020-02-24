using System.Collections.Generic;
using Domain.Models;
using MediatR;

namespace Application.Search.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<IEnumerable<UserVm>>
    {
        public string Term { get; set; }
    }
}