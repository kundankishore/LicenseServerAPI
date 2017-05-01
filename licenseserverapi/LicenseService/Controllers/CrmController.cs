using LicenseService.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using LicenseService.Filters;

namespace LicenseService.Controllers
{
    [BasicAuthenticationFilter]
    public class CrmController : ApiController
    {
        [HttpGet]
        [Route(@"accounts/{changedSince:DateTime}")]
        public IEnumerable<Account> GetAccounts([FromUri] DateTime changedSince)
        {
            var service = new BusinessLogic.LicenseService();
            var accounts = service.GetAccounts(changedSince);
            return accounts;
        }

        [HttpGet]
        [Route(@"accounts")]
        public IEnumerable<Account> GetAccounts()
        {
            var service = new BusinessLogic.LicenseService();
            var accounts = service.GetAccounts();
            return accounts;
        }

        [HttpGet]
        [Route(@"contracts/{changedSince:DateTime}")]
        public IEnumerable<Contract> GetContracts([FromUri] DateTime changedSince)
        {
            var service = new BusinessLogic.LicenseService();
            var contracts = service.GetContracts(changedSince);
            return contracts;
        }

        [HttpGet]
        [Route(@"contracts/")]
        public IEnumerable<Contract> GetContracts()
        {
            var service = new BusinessLogic.LicenseService();
            var contracts = service.GetContracts();
            return contracts;
        }

        [HttpGet]
        [Route(@"products/{changedSince:DateTime}")]
        public IEnumerable<Product> GetProducts([FromUri] DateTime changedSince)
        {
            var service = new BusinessLogic.LicenseService();
            var products = service.GetProducts(changedSince);
            return products;
        }

        [HttpGet]
        [Route(@"products/")]
        public IEnumerable<Product> GetProducts()
        {
            var service = new BusinessLogic.LicenseService();
            var products = service.GetProducts();
            return products;
        }
    }
}
