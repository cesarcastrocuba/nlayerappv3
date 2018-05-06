namespace NLayerApp.Application.Seedwork.Common
{
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Domain.Seedwork.Specification;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    public interface IService<TEntity, TEntityDTO> : IDisposable
        where TEntity : Entity
        where TEntityDTO : Entity
    {
        TEntity Add(TEntity item);
        TEntityDTO Add(TEntityDTO item);

        IEnumerable<TEntity> Add(IEnumerable<TEntity> items);
        IList<TEntityDTO> Add(IList<TEntityDTO> items);

        Task<TEntity> AddAsync(TEntity item);
        Task<TEntityDTO> AddAsync(TEntityDTO item);

        Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items);
        Task<IList<TEntityDTO>> AddAsync(IList<TEntityDTO> item);

        void Remove(TEntity item);
        void Remove(TEntityDTO item);
        void Remove(object id);

        void Remove(IEnumerable<TEntity> items);
        void Remove(IList<TEntityDTO> items);
        void Remove(IEnumerable<object> ids);

        Task RemoveAsync(TEntity item);
        Task RemoveAsync(TEntityDTO item);
        Task RemoveAsync(object id);

        Task RemoveAsync(IEnumerable<TEntity> items);
        Task RemoveAsync(IList<TEntityDTO> items);
        Task RemoveAsync(IEnumerable<object> ids);

        TEntity Modify(TEntity item);
        TEntityDTO Modify(TEntityDTO item);

        IEnumerable<TEntity> Modify(IEnumerable<TEntity> items);
        IList<TEntityDTO> Modify(IList<TEntityDTO> items);

        TEntity Modify(object id, TEntity item);
        TEntityDTO Modify(object id, TEntityDTO item);

        Task<TEntity> ModifyAsync(TEntity item);
        Task<TEntityDTO> ModifyAsync(TEntityDTO item);

        Task<IEnumerable<TEntity>> ModifyAsync(IEnumerable<TEntity> items);
        Task<IList<TEntityDTO>> ModifyAsync(IList<TEntityDTO> items);

        Task<TEntity> ModifyAsync(object id, TEntity item);
        Task<TEntityDTO> ModifyAsync(object id, TEntityDTO item);

        void Refresh(TEntity item);
        void Refresh(TEntityDTO item);
        
        TEntity Get(object id);
        TEntityDTO GetDTO(object id);

        Task<TEntity> GetAsync(object id);
        Task<TEntityDTO> GetDTOAsync(object id);

        IEnumerable<TEntity> GetAll();
        IList<TEntityDTO> GetAllDTO();

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IList<TEntityDTO>> GetAllDTOAsync();

        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);
        IList<TEntityDTO> AllMatchingDTO(ISpecification<TEntity> specification);

        Task<IEnumerable<TEntity>> AllMatchingAsync(ISpecification<TEntity> specification);
        Task<IList<TEntityDTO>> AllMatchingDTOAsync(ISpecification<TEntity> specification);

        IEnumerable<TEntity> AllMatching<KProperty>(ISpecification<TEntity> specification, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        IList<TEntityDTO> AllMatchingDTO<KProperty>(ISpecification<TEntity> specification, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        Task<IEnumerable<TEntity>> AllMatchingAsync<KProperty>(ISpecification<TEntity> specification, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        Task<IList<TEntityDTO>> AllMatchingDTOAsync<KProperty>(ISpecification<TEntity> specification, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        TEntity FirstMatching(ISpecification<TEntity> specification);
        TEntityDTO FirstMatchingDTO(ISpecification<TEntity> specification);

        Task<TEntity> FirstMatchingAsync(ISpecification<TEntity> specification);
        Task<TEntityDTO> FirstMatchingDTOAsync(ISpecification<TEntity> specification);

        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        IList<TEntityDTO> GetPagedDTO<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        Task<IEnumerable<TEntity>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        Task<IList<TEntityDTO>> GetPagedDTOAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
        IList<TEntityDTO> GetFilteredDTO(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter);
        Task<IList<TEntityDTO>> GetFilteredDTOAsync(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        IList<TEntityDTO> GetFilteredDTO<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        Task<IEnumerable<TEntity>> GetFilteredAsync<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        Task<IList<TEntityDTO>> GetFilteredDTOAsync<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
    }
}
