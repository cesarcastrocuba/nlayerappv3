using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.DTO;
using NLayerApp.Application.MainBoundedContext.BlogModule.Services;
using NLayerApp.DistributedServices.MainBoundedContext.Filters;
using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
{
    /// <summary>
    /// The blogs controller contains all operations for the Post entity
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class PostsController : ApiController<Post, PostDTO>
    {
        private readonly IPostsService _postsService;
        public PostsController(IPostsService postsService)
            : base(postsService)
        {
            _postsService = postsService;
        }

        //GET api/posts
        //GET api/posts?q=api
        [HttpGet("GetFiltered")]
        public async Task<IEnumerable<PostDTO>> Get(string q)
        {
            return await _postsService.GetAllDTOAsync(q);
        }
    }
}
