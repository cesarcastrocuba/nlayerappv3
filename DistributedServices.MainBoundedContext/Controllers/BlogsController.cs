using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.DTO;
using NLayerApp.Application.MainBoundedContext.BlogModule.Services;
using NLayerApp.DistributedServices.MainBoundedContext.Filters;
using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
{
    /// <summary>
    /// The blogs controller contains all operations for the Blog entity
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class BlogsController : AsyncApiController<Blog,BlogDTO>    
    {
        private readonly IBlogsService _blogsService;

        public BlogsController(
            IBlogsService blogsService
            ):base(blogsService)
        {
            _blogsService = blogsService;
        }

        // GET api/blogs/5/posts
        [HttpGet("{id}/[action]")]
        public async Task<IEnumerable<PostDTO>> Posts(int id)
        {
            return await _blogsService.GetPostsDTOAsync(id);
        }
    }
}
