using MediatR;

namespace Application.Home.Queries.GetHomePage
{
    public class GetHomePageQuery : IRequest<GetGomePageResponse>
    {
        public int Page { get; set; }
    }
}
