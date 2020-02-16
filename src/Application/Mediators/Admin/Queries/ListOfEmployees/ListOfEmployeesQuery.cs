using MediatR;

namespace Application.Admin.Queries.ListOfEmployees
{
    public class ListOfEmployeesQuery : IRequest<ListOfEmployeesResponse>
    {
        public int Page { get; set; }
    }
}
