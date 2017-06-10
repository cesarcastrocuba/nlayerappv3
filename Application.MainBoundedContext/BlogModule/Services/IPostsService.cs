namespace NLayerApp.Application.MainBoundedContext.BlogModule.Services
{
    using NLayerApp.Application.MainBoundedContext.Common;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IPostsService : IService<Post,PostDTO>, IDisposable
    {
        Task<List<PostDTO>> GetAllDTOAsync(string q);
    }
}
