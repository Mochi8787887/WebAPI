using HW_FP_122484.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HW_FP_122484.Services
{
    public class ClaimAccessor
    {
        public readonly Settings _settings;

        public readonly IHttpContextAccessor _accessor;

        public ClaimAccessor(IOptions<Settings> settings, IHttpContextAccessor accessor)
        {
            _settings = settings.Value;
            _accessor = accessor;
        }
    }
}