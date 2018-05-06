namespace NLayerApp.Application.BlogBoundedContext.BlogModule.Services
{
    using NLayerApp.Application.BlogBoundedContext.DTO;
    using NLayerApp.Application.Seedwork.Common;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IBlogsService : IService<Blog,BlogDTO>, IDisposable
    {
        Task<IList<PostDTO>> GetPostsDTOAsync(int id);        
    }
}
