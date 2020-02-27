using MediatR;

namespace Application.Admin.Queries.GetEmployee
{
    public class GetEmployeeQuery : IRequest<GetEmployeeResponse>
    {
        public string Id { get; set; }
    }
}
