using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.BankingModule.Services;
using NLayerApp.Application.MainBoundedContext.DTO;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
{
    [Route("api/[controller]")]
    public class BankAccounts : Controller, IDisposable
    {
        readonly IBankAppService _bankAppService;

        public BankAccounts(IBankAppService bankAppService)
        {
            _bankAppService = bankAppService;
        }

        // GET: api/bankaccounts
        [HttpGet]
        public IEnumerable<BankAccountDTO> Get()
        {
            return _bankAppService.FindBankAccounts();
        }

        // GET api/bankaccounts/getactivities/5
        [HttpGet("GetActivities/{id}")]
        public IEnumerable<BankActivityDTO> Get(Guid bankAccountId)
        {
            return _bankAppService.FindBankAccountActivities(bankAccountId);
        }

        // POST api/bankaccounts
        [HttpPost]
        public BankAccountDTO Post([FromBody]BankAccountDTO newBankAccount)
        {
            return _bankAppService.AddBankAccount(newBankAccount);
        }

        // PUT api/bankaccounts/lock/5
        [HttpPut("{id}")]
        public bool Lock(Guid bankAccountId)
        {
            return _bankAppService.LockBankAccount(bankAccountId);
        }

        // PUT api/bankaccounts/performtransfer
        [HttpPut]
        public void PerformTransfer([FromBody]BankAccountDTO from, [FromBody]BankAccountDTO to, [FromBody]decimal amount)
        {
            _bankAppService.PerformBankTransfer(from, to, amount);
        }

        #region IDisposable Members
        public void Dispose()
        {
            //dispose all resources
            _bankAppService.Dispose();
        }
        #endregion
    }
}
