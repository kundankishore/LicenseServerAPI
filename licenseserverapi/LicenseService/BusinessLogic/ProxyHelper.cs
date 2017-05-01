using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace LicenseService.BusinessLogic
{
    public static class ProxyHelper
    {
        private static IOrganizationService _service;
        public static IOrganizationService Service
        {
            get { return _service ?? (_service = new OrganizationService(GetCrmConnection())); }
        }


        public static CrmConnection GetCrmConnection()
        {
            var crmConnectionString = _connectionString;
            var connection = CrmConnection.Parse(crmConnectionString);
            return connection;
        }

        private static string _connectionString = System.Configuration.ConfigurationManager.AppSettings["CRMConnectionCredentials"];
             
    }
}
