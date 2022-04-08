using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Manage.API.Models.v1.Requests.PostCodes;
using Manage.API.Models.v1.Responses.PostCodes;
using Manage.Repository.Data;
using Microsoft.AspNetCore.Mvc;

namespace Manage.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostcodesController : ControllerBase
    {
        
        private readonly ManageDataContext _context;
        private readonly IMapper _mapper;
        private readonly Interfaces.IPostCodesRepository _postCodesRepository;

        public PostcodesController(ManageDataContext context, IMapper mapper, Interfaces.IPostCodesRepository postCodesRepository)
        {
            _context = context;
            _mapper = mapper;
            _postCodesRepository = postCodesRepository;
        }
        
        [HttpPost("FindByAny")]
        public async Task<ActionResult<List<PostCodeResponse>>> FindByAny([FromBody]PostCodeQueryRequest searchQuery)
        {
            return await _postCodesRepository.FindPostCodesByAny(searchQuery.Query);
        }

        [HttpPost("FindBySlug")]
        public async Task<ActionResult<List<PostCodeResponse>>> FindBySlug([FromBody]PostCodeQueryRequest searchQuery)
        {
            return await _postCodesRepository.FindPostCodesBySlug(searchQuery.Query);
        }
    }
}