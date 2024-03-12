using HW_FP_122484.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HW_FP_122484
{
    public class AuthoriaztionFilter : Attribute, IAsyncAuthorizationFilter
    {
        public readonly ClaimAccessor _claimAccessor;


        public AuthoriaztionFilter(ClaimAccessor claimAccessor)
        {
            _claimAccessor = claimAccessor;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {

                if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                    return;
                var token = context.HttpContext.Request.Headers["Token"];

                var checkToken = _claimAccessor._settings.Token;

                //if (token != "123456")
                //    context.Result = new UnauthorizedResult();

                if (token != checkToken)
                    context.Result = new UnauthorizedResult();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
