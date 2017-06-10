namespace NLayerApp.Application.MainBoundedContext.BlogModule.Services
{
    using NLayerApp.Application.MainBoundedContext.Common;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IBlogsService : IService<Blog,BlogDTO>, IDisposable
    {
        Task<IList<PostDTO>> GetPostsDTOAsync(int id);        
    }
}
