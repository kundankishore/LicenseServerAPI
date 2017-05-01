using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using LicenseService.Models;

namespace LicenseService.BusinessLogic
{
    public class LicenseService
    {
        public IEnumerable<Account> GetAccounts(DateTime changedSince)
        {
            return Accounts(FetchHelper.FetchXmlForGetAccountsSince.Replace("changedsince",
                Convert.ToString(changedSince)));
        }

        public IEnumerable<Account> GetAccounts()
        {
            return Accounts(FetchHelper.FetchXmlForGetAccounts);
        }

        public IEnumerable<Contract> GetContracts(DateTime changedSince)
        {
            return
                Contracts(FetchHelper.FetchXmlForGetContractsSince.Replace("changedsince",
                    Convert.ToString(changedSince)));
        }

        public IEnumerable<Contract> GetContracts()
        {
            return Contracts(FetchHelper.FetchXmlForGetContracts);
        }

        public IEnumerable<Product> GetProducts(DateTime changedSince)
        {
            return
                Products(FetchHelper.FetchXmlForGetProductsSince.Replace("changedsince", Convert.ToString(changedSince)));
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products(FetchHelper.FetchXmlForGetProducts);
        }

        private static IEnumerable<Product> Products(string productsxml)
        {
            IList<Product> cnt = new List<Product>();

            var service = ProxyHelper.Service;

            List<EntityCollection> productCollection = new List<EntityCollection>();

            // Defining the fetch attributes.
            // Set the number of records per page to retrieve.
            int fetchCount = 50000;
            // Initialize the page number.
            int pageNumber = 1;

            // Specifing the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            while (true)
            {
                // Build fetchXml string with the placeholders.
                var xml = FetchHelper.CreateXml(productsxml, pagingCookie, pageNumber, fetchCount);

                // Excute the fetch query and get the xml result.
                var productFetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                var ent = ((RetrieveMultipleResponse) service.Execute(productFetchRequest)).EntityCollection;

                productCollection.Add(ent);


                // Check for morerecords, if it returns 1.
                if (ent.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = ent.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }
            var tupleList = new List<Tuple<Guid, Guid, string, string>>();
            foreach (var enty in productCollection)
            {
                foreach (var product in enty.Entities)
                {
                    tupleList.Add(
                        Tuple.Create(
                            product.GetAttributeValue<EntityReference>("productid") != null
                                ? product.GetAttributeValue<EntityReference>("productid").Id
                                : Guid.Empty,
                            product.GetAttributeValue<EntityReference>("associatedproduct") != null
                                ? product.GetAttributeValue<EntityReference>("associatedproduct").Id
                                : Guid.Empty,
                            product.GetAttributeValue<AliasedValue>("AssociatedProd.dbc_productgroupid") != null
                                ? ((EntityReference)
                                        (product.GetAttributeValue<AliasedValue>("AssociatedProd.dbc_productgroupid").Value))
                                    .Name
                                : string.Empty,
                            product.GetAttributeValue<AliasedValue>("AssociatedProd.name") != null
                                ? Convert.ToString(product.GetAttributeValue<AliasedValue>("AssociatedProd.name").Value)
                                : string.Empty));
                }
            }

            foreach (var tuple in tupleList)
            {
                if (cnt.All(x => x.ProductId != tuple.Item1))
                {
                    var prd = new Product
                    {
                        ProductId = tuple.Item1,
                        SubProducts = new List<SubProducts>()
                    };

                    var allsubproducts = tupleList.Where(x => x.Item1 == tuple.Item1);
                    foreach (var subproduct in allsubproducts)
                    {
                        var subprod = new SubProducts
                        {
                            SubProductId = subproduct.Item2,
                            Name = subproduct.Item4,
                            ProductGroup = subproduct.Item3
                        };
                        prd.SubProducts.Add(subprod);
                    }
                    if (prd.SubProducts.Count > 0)
                    {
                        prd.ProductId = tuple.Item1;
                        cnt.Add(prd);
                    }
                }
            }
            return cnt;
        }

        private static IEnumerable<Contract> Contracts(string contractssxml)
        {
            IList<Contract> cnt = new List<Contract>();

            var service = ProxyHelper.Service;

            List<EntityCollection> contractCollection = new List<EntityCollection>();

            // Defining the fetch attributes.
            // Set the number of records per page to retrieve.
            int fetchCount = 50000;
            // Initialize the page number.
            int pageNumber = 1;

            // Specifing the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            while (true)
            {
                // Build fetchXml string with the placeholders.
                var xml = FetchHelper.CreateXml(contractssxml, pagingCookie, pageNumber, fetchCount);

                // Excute the fetch query and get the xml result.
                var contractFetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                var ent = ((RetrieveMultipleResponse) service.Execute(contractFetchRequest)).EntityCollection;

                contractCollection.Add(ent);


                // Check for morerecords, if it returns 1.
                if (ent.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = ent.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }


            var tupleList = new List<Tuple<Guid, Guid, DateTime, DateTime, Guid, string>>();
            foreach (var enty in contractCollection)
            {
                foreach (var contract in enty.Entities)
                {
                    tupleList.Add(Tuple.Create(contract.GetAttributeValue<EntityReference>("dbc_customerid") != null
                            ? contract.GetAttributeValue<EntityReference>("dbc_customerid").Id
                            : Guid.Empty,
                        contract.GetAttributeValue<AliasedValue>("contractdetails.productid") != null
                            ? ((EntityReference)
                                (contract.GetAttributeValue<AliasedValue>("contractdetails.productid").Value)).Id
                            : Guid.Empty,
                        contract.GetAttributeValue<AliasedValue>("contractdetails.activeon") != null
                            ? Convert.ToDateTime(
                                contract.GetAttributeValue<AliasedValue>("contractdetails.activeon").Value)
                            : DateTime.MinValue,
                        contract.GetAttributeValue<AliasedValue>("contractdetails.expireson") != null
                            ? Convert.ToDateTime(
                                contract.GetAttributeValue<AliasedValue>("contractdetails.expireson").Value)
                            : DateTime.MinValue,
                        contract.GetAttributeValue<EntityReference>("dbc_supportedbyid") != null
                            ? contract.GetAttributeValue<EntityReference>("dbc_supportedbyid").Id
                            : Guid.Empty,
                        contract.GetAttributeValue<AliasedValue>("contractdetails.title") != null
                            ? Convert.ToString(contract.GetAttributeValue<AliasedValue>("contractdetails.title").Value)
                            : string.Empty));
                }
            }

            foreach (var tuple in tupleList)
            {
                if (cnt.All(x => x.CrmId != tuple.Item1))
                {
                    var contract = new Contract
                    {
                        CrmId = tuple.Item1,
                        SubList = new List<SubList>()
                    };
                    var allsubcontracts = tupleList.Where(x => x.Item1 == tuple.Item1);
                    foreach (var subcontracts in allsubcontracts)
                    {
                        var sublist = new SubList
                        {
                            ProductId = subcontracts.Item2,
                            StartDate = subcontracts.Item3,
                            EndDate = subcontracts.Item4,
                            SupportedBy = subcontracts.Item5,
                            ProductName = subcontracts.Item6
                        };
                        contract.SubList.Add(sublist);
                    }
                    if (contract.SubList.Count > 0)
                    {
                        contract.CrmId = tuple.Item1;
                        cnt.Add(contract);
                    }
                }
            }
            return cnt;
        }

        private static IEnumerable<Account> Accounts(string accountsxml)
        {
            IList<Account> cnt = new List<Account>();

            var service = ProxyHelper.Service;

            List<EntityCollection> accountCollection = new List<EntityCollection>();

            // Defining the fetch attributes.
            // Set the number of records per page to retrieve.
            int fetchCount = 50000;
            // Initialize the page number.
            int pageNumber = 1;

            // Specifing the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            while (true)
            {
                // Build fetchXml string with the placeholders.
                var xml = FetchHelper.CreateXml(accountsxml, pagingCookie, pageNumber, fetchCount);

                // Excute the fetch query and get the xml result.
                var accountFetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                var ent = ((RetrieveMultipleResponse) service.Execute(accountFetchRequest)).EntityCollection;

                accountCollection.Add(ent);


                // Check for morerecords, if it returns 1.
                if (ent.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = ent.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }

            foreach (var enty in accountCollection)
            {
                foreach (var account in enty.Entities)
                {
                    var acct = new Account
                    {
                        AccountName = account.GetAttributeValue<string>("name") != null
                            ? account.GetAttributeValue<string>("name")
                            : string.Empty,
                        AddressStreet1 = account.GetAttributeValue<string>("address1_line1") !=
                                         null
                            ? account.GetAttributeValue<string>("address1_line1")
                            : string.Empty,
                        AddressStreet2 = account.GetAttributeValue<string>("address1_line2") !=
                                         null
                            ? account.GetAttributeValue<string>("address1_line2")
                            : string.Empty,
                        AddressStreet3 = account.GetAttributeValue<string>("address1_line3") !=
                                         null
                            ? account.GetAttributeValue<string>("address1_line3")
                            : string.Empty,
                        City = account.GetAttributeValue<string>("address1_city") != null
                            ? account.GetAttributeValue<string>("address1_city")
                            : string.Empty,
                        County = account.GetAttributeValue<string>("address1_county") !=
                                 null
                            ? account.GetAttributeValue<string>("address1_county")
                            : string.Empty,
                        ZipOrPostCode = account.GetAttributeValue<string>("address1_postalcode") != null
                            ? account.GetAttributeValue<string>("address1_postalcode")
                            : string.Empty,
                        Country = account.GetAttributeValue<OptionSetValue>("crs_country") !=
                                  null
                            ? account.FormattedValues["crs_country"]
                            : string.Empty,
                        CustomerType = account.GetAttributeValue<OptionSetValue>("dbc_accountcustomertype") != null
                            ? account.FormattedValues["dbc_accountcustomertype"]
                            : string.Empty,
                        CustomerSubType = account.GetAttributeValue<OptionSetValue>("dbc_customersubtype") != null
                            ? account.FormattedValues["dbc_customersubtype"]
                            : string.Empty,
                        Governance = account.GetAttributeValue<OptionSetValue>("crs_governance") != null
                            ? account.FormattedValues["crs_governance"]
                            : string.Empty,
                        LaNumber = account.GetAttributeValue<string>("dbc_leanumber") != null
                            ? account.GetAttributeValue<string>("dbc_leanumber")
                            : string.Empty,
                        DfeNumber = account.GetAttributeValue<string>("dbc_dfenumber") != null
                            ? account.GetAttributeValue<string>("dbc_dfenumber")
                            : string.Empty,
                        Email = account.GetAttributeValue<string>("emailaddress1") != null
                            ? account.GetAttributeValue<string>("emailaddress1")
                            : string.Empty,
                        Mainphone = account.GetAttributeValue<string>("telephone1") != null
                            ? account.GetAttributeValue<string>("telephone1")
                            : string.Empty,
                        LowerAgeRange = account.GetAttributeValue<string>("new_loweragerange") != null
                            ? account.GetAttributeValue<string>("new_loweragerange")
                            : string.Empty,
                        UpperAgeRange = account.GetAttributeValue<string>("new_upperagerange") != null
                            ? account.GetAttributeValue<string>("new_upperagerange")
                            : string.Empty,
                        OfstedURN = account.GetAttributeValue<string>("new_ofstedurn") != null
                            ? account.GetAttributeValue<string>("new_ofstedurn")
                            : string.Empty,
                        Id = account.Id,
                        Website = account.GetAttributeValue<string>("websiteurl") != null
                            ? account.GetAttributeValue<string>("websiteurl")
                            : string.Empty,
                        SchoolGroupGuid = account.GetAttributeValue<EntityReference>("dbc_schoolgroupid") != null
                            ? account.GetAttributeValue<EntityReference>("dbc_schoolgroupid").Id
                            : Guid.Empty,
                        LocalAuthority = account.GetAttributeValue<EntityReference>("dbc_leaid") != null
                            ? account.GetAttributeValue<EntityReference>("dbc_leaid").Id
                            : Guid.Empty,
                        Status = account.GetAttributeValue<OptionSetValue>("statecode") !=
                                 null
                            ? account.FormattedValues["statecode"]
                            : string.Empty
                    };

                    cnt.Add(acct);
                }
            }

            return cnt;
        }
    }
}
