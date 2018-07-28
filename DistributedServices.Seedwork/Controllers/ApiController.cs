using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.Seedwork.Common;
using NLayerApp.Domain.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace NLayerApp.DistributedServices.Seedwork.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiController<TEntity,TEntityDTO> : Controller where TEntity : Entity where TEntityDTO : Entity
    {
        protected IService<TEntity,TEntityDTO> _service;

        public ApiController(IService<TEntity, TEntityDTO> service)
        {
            _service = service;
        }

        // GET api/Entity
        [HttpGet]
        public virtual IEnumerable<TEntityDTO> Get()
        {
            return _service.GetAllDTO();
        }

        // GET api/Entity/GetById/id
        [HttpGet("GetById/{id}")]
        public virtual TEntityDTO GetById(int id)
        {
            return _service.GetDTO(id);
        }

        // POST api/Entity
        [HttpPost]
        public virtual TEntityDTO Post([FromBody]TEntityDTO entityDTO)
        {
            return _service.Add(entityDTO);
        }

        // POST api/Entity/PostItems
        [HttpPost("PostItems")]
        public virtual IEnumerable<TEntityDTO> PostItems([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
        {
            return _service.Add(entitiesDTO.ToList());
        }

        // PUT api/Entity
        [HttpPut()]
        public virtual TEntityDTO Put([FromBody]TEntityDTO entityDTO)
        {
            return _service.Modify(entityDTO);
        }

        // PUT api/Entity/PutWithId/5
        [HttpPut("PutWithId/{id}")]
        public virtual TEntityDTO PutWithId(int id, [FromBody]TEntityDTO entityDTO)
        {
            return _service.Modify(id, entityDTO);
        }

        // PUT api/Entity/PutItems
        [HttpPut("PutItems")]
        public virtual IEnumerable<TEntityDTO> PutItems([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
        {
            return _service.Add(entitiesDTO.ToList());
        }

        // DELETE api/Entity/5
        [HttpDelete("{id}")]
        public virtual void Delete(int id)
        {
            _service.Remove(id);
        }

        // DELETE api/Entity/DeleteItems
        [HttpDelete("DeleteItems")]
        public virtual void DeleteItems(IEnumerable<int> ids)
        {
            _service.Remove(ids);
        }
    }
}
