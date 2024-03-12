using HW_FP.Data.TrainDB122484.Data;
using HW_FP_122484.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HW_FP_122484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilterAttribute(typeof(AuthoriaztionFilter))]
    public class APIControllerBase : ControllerBase
    {
        public readonly ClaimAccessor _claimAccessor;
        public readonly TrainDB122484Context _TrainDB122484Context;

        public APIControllerBase()
        {

        }

        public APIControllerBase(ClaimAccessor claimAccessor, TrainDB122484Context trainDB122484Context)
        {
            _claimAccessor = claimAccessor;
            _TrainDB122484Context = trainDB122484Context;
        }

    }
}