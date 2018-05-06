using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.BlogBoundedContext.BlogModule.Services;
using NLayerApp.Application.BlogBoundedContext.DTO;
using NLayerApp.DistributedServices.Seedwork.Controllers;
using NLayerApp.DistributedServices.Seedwork.Filters;
using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLayerApp.DistributedServices.BlogBoundedContext.Controllers
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
