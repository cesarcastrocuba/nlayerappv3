using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.Common;
using NLayerApp.Domain.Seedwork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
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

        // GET api/Entity/id
        [HttpGet("{id}")]
        public virtual TEntityDTO Get(int id)
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
        [HttpPost("AddItems")]
        public virtual IEnumerable<TEntityDTO> Post([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
        {
            return _service.Add(entitiesDTO.ToList());
        }

        // PUT api/Entity
        [HttpPut()]
        public virtual TEntityDTO Put([FromBody]TEntityDTO entityDTO)
        {
            return _service.Modify(entityDTO);
        }
        
        // PUT api/Entity/5
        [HttpPut("ModifyWithId/{id}")]
        public virtual TEntityDTO Put(int id, [FromBody]TEntityDTO entityDTO)
        {
            return _service.Modify(id, entityDTO);
        }

        // PUT api/Entity/PutItems
        [HttpPut("ModifyItems")]
        public virtual IEnumerable<TEntityDTO> Put([FromBody]IEnumerable<TEntityDTO> entitiesDTO)
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
        public virtual void Delete(IEnumerable<int> ids)
        {
            _service.Remove(ids);
        }
    }
}
