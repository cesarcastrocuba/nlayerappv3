using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.Seedwork.Common;
using NLayerApp.Domain.Seedwork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLayerApp.DistributedServices.Seedwork.Controllers
{
    [Route("api/[controller]")]
    public abstract class AsyncApiController<TEntity,TEntityDTO> : Controller where TEntity : Entity where TEntityDTO : Entity
    {
        protected IService<TEntity,TEntityDTO> _service;

        public AsyncApiController(IService<TEntity, TEntityDTO> service)
        {
            _service = service;
        }

        // GET api/Entity
        [HttpGet]
        public virtual async Task<IEnumerable<TEntityDTO>> Get()
        {
            return await _service.GetAllDTOAsync();
        }

        // GET api/Entity/id
        [HttpGet("GetById/{id}")]
        public virtual async Task<TEntityDTO> GetById(int id)
        {
            return await _service.GetDTOAsync(id);
        }

        // POST api/Entity
        [HttpPost]
        public virtual async Task<TEntityDTO> Post([FromBody]TEntityDTO entityDTO)
        {
            return await _service.AddAsync(entityDTO);
        }

        // POST api/Entity/PostItems
        [HttpPost("PostItems")]
        public virtual async Task<IEnumerable<TEntityDTO>> PostItems([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
        {
            return await _service.AddAsync(entitiesDTO.ToList());
        }

        //// PUT api/Entity
        [HttpPut()]
        public virtual async Task<TEntityDTO> Put([FromBody]TEntityDTO entityDTO)
        {
            return await _service.ModifyAsync(entityDTO);
        }

        // PUT api/Entity/PutWithId/5
        [HttpPut("PutWithId/{id}")]
        public virtual async Task<TEntityDTO> PutWithId(int id, [FromBody]TEntityDTO entityDTO)
        {
            return await _service.ModifyAsync(id, entityDTO);
        }

        // PUT api/Entity/PutItems
        [HttpPut("PutItems")]
        public virtual async Task<IEnumerable<TEntityDTO>> PutItems([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
        {
            return await _service.AddAsync(entitiesDTO.ToList());
        }

        // DELETE api/Entity/5
        [HttpDelete("{id}")]
        public virtual async Task Delete(int id)
        {
            await _service.RemoveAsync(id);
        }

        // DELETE api/Entity/DeleteItems
        [HttpDelete("DeleteItems")]
        public virtual async Task DeleteItems(IEnumerable<int> ids)
        {
            await _service.RemoveAsync(ids);
        }
    }
}
