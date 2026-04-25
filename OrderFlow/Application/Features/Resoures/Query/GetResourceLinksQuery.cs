using Application.Wrappers;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Resoures.Query
{
    public class GetResourceLinksQuery : IRequest<Response<List<ResourceLinkDto>>>
    {
        public class GetResourceLinksQueryHandler : IRequestHandler<GetResourceLinksQuery, Response<List<ResourceLinkDto>>>
        {
            private readonly ResourceLinkSettings _resourceSettings;

            public GetResourceLinksQueryHandler(IOptions<ResourceLinkSettings> resourceSettings)
            {
                _resourceSettings = resourceSettings.Value;
            }

            public async Task<Response<List<ResourceLinkDto>>> Handle(GetResourceLinksQuery request, CancellationToken cancellationToken)
            {
                var response = new List<ResourceLinkDto>
                {
                    new ResourceLinkDto
                    {
                        Label = "Advocate",
                        Link = _resourceSettings.AdvocateLink,
                        Pass = _resourceSettings.AdvocatePass,
                    },
                    new ResourceLinkDto
                    {
                        Label = "Academic",
                        Link = _resourceSettings.AcademicLink,
                        Pass = _resourceSettings.AcademicPass,
                    }
                };

                return new Response<List<ResourceLinkDto>>(response, "Successful"); 
            }
        }
    }
}
