namespace NLayerApp.Application.BlogBoundedContext.BlogModule.Services
{
    using NLayerApp.Application.BlogBoundedContext.DTO;
    using NLayerApp.Application.Seedwork.Common;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IPostsService : IService<Post,PostDTO>, IDisposable
    {
        Task<List<PostDTO>> GetAllDTOAsync(string q);
    }
}
