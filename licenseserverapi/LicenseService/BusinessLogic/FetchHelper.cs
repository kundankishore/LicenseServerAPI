using System;
using System.IO;
using System.Text;
using System.Xml;

namespace LicenseService.BusinessLogic
{
    public static class FetchHelper
    {
        public static string CreateXml(string xml, string cookie, int page, int count)
        {
            var stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            // Load document
            var doc = new XmlDocument();
            doc.Load(reader);

            return CreateXml(doc, cookie, page, count);
        }

        public static string CreateXml(XmlDocument doc, string cookie, int page, int count)
        {
            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }

        #region Below is the Fetch XML used for GetAccounts Method using modified date filter

        public static string FetchXmlForGetAccountsSince = @"<?xml version='1.0'?>
            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='name' />
    <attribute name='address1_city' />
    <attribute name='telephone1' />
    <attribute name='dbc_leanumber' />
    <attribute name='dbc_dfenumber' />
    <attribute name='address1_postalcode' />
    <attribute name='accountid' />
    <attribute name='websiteurl' />
    <attribute name='new_upperagerange' />
    <attribute name='new_loweragerange' />
    <attribute name='dbc_leaid' />
    <attribute name='dbc_accountcustomertype' />
    <attribute name='dbc_customersubtype' />
    <attribute name='address1_line3' />
    <attribute name='address1_line2' />
    <attribute name='address1_line1' />
    <attribute name='address1_county' />
    <attribute name='emailaddress1' />
    <attribute name='crs_country' />
    <attribute name='new_ofstedurn' />
    <attribute name='dbc_schoolgroupid' />
    <attribute name='statecode' />
    <attribute name='crs_governance' />
    <order attribute='name' descending='false' />
    <filter type='and'>
      <condition attribute='modifiedon' operator='on-or-after' value='changedsince' />
      <condition attribute='dbc_accountcustomertype' operator='not-in'>
        <value>865550004</value>
        <value>865550011</value>
      </condition>
      <condition attribute='statecode' operator='in'>
        <value>0</value>
        <value>1</value>
      </condition>
    </filter>
  </entity>
</fetch>";

        #endregion

        #region Below is the Fetch XML used for GetAccounts Method

        public static string FetchXmlForGetAccounts = @"<?xml version='1.0'?>
            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='name' />
    <attribute name='address1_city' />
    <attribute name='telephone1' />
    <attribute name='dbc_leanumber' />
    <attribute name='dbc_dfenumber' />
    <attribute name='address1_postalcode' />
    <attribute name='accountid' />
    <attribute name='websiteurl' />
    <attribute name='new_upperagerange' />
    <attribute name='new_loweragerange' />
    <attribute name='dbc_leaid' />
    <attribute name='dbc_accountcustomertype' />
    <attribute name='dbc_customersubtype' />
    <attribute name='address1_line3' />
    <attribute name='address1_line2' />
    <attribute name='address1_line1' />
    <attribute name='address1_county' />
    <attribute name='emailaddress1' />
    <attribute name='crs_country' />
    <attribute name='new_ofstedurn' />
    <attribute name='dbc_schoolgroupid' />
    <attribute name='statecode' />
    <attribute name='crs_governance' />
    <order attribute='name' descending='false' />
    <filter type='and'>
      <condition attribute='dbc_accountcustomertype' operator='not-in'>
        <value>865550004</value>
        <value>865550011</value>
      </condition>
      <condition attribute='statecode' operator='in'>
        <value>0</value>
      </condition>
    </filter>
  </entity>
</fetch>";

        #endregion

        #region Below is the Fetch XML used for GetProducts Method using modified date as filter

        public static string FetchXmlForGetProductsSince = @"<?xml version='1.0'?>
<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='productassociation'>
    <attribute name='productid' />
    <attribute name='associatedproduct' />
    <order attribute='productid' descending='false' />
    <filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    <link-entity name='product' from='productid' to='productid' alias='SupProd'>
      <attribute name='name' />
      <filter type='and'>
        <condition attribute='dbc_issupportproduct' operator='eq' value='1' />
        <condition attribute='modifiedon' operator='on-or-after' value='changedsince' />
      </filter>
    </link-entity>
                <link-entity name='product' from='productid' to='associatedproduct' alias='AssociatedProd'>
      <attribute name='productnumber' />
      <attribute name='dbc_productgroupid' />
      <attribute name='name' />
<filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    </link-entity>
  </entity>
</fetch>";

        #endregion

        #region Below is the Fetch XML used for GetProducts Method

        public static string FetchXmlForGetProducts = @"<?xml version='1.0'?>

<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='productassociation'>
    <attribute name='productid' />
    <attribute name='associatedproduct' />
    <order attribute='productid' descending='false' />
    <filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    <link-entity name='product' from='productid' to='productid' alias='SupProd'>
      <attribute name='name' />
      <filter type='and'>
        <condition attribute='dbc_issupportproduct' operator='eq' value='1' />
      </filter>
    </link-entity>
                <link-entity name='product' from='productid' to='associatedproduct' alias='AssociatedProd'>
      <attribute name='productnumber' />
      <attribute name='dbc_productgroupid' />
      <attribute name='name' />
<filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    </link-entity>
  </entity>
</fetch>";

        #endregion

        #region Below is the Fetch XML used for GetContracts Method using modified date as filter

        public static string FetchXmlForGetContractsSince = @"<?xml version='1.0'?>

<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
  <entity name='dbc_amcontractline'>
    <attribute name='dbc_supportedbyid' />
    <attribute name='dbc_customerid' />
    <attribute name='dbc_amcontractlineid' />
    <order attribute='dbc_customerid' descending='false' />
    <filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    <link-entity name='account' from='accountid' to='dbc_customerid' alias='ay'>
      <filter type='and'>
        <condition attribute='statecode' operator='eq' value='0' />
      </filter>
      <link-entity name='dbc_amcontractline' from='dbc_customerid' to='accountid' alias='az'>
        <filter type='and'>
          <condition attribute='modifiedon' operator='on-or-after' value='changedsince' />
        </filter>
      </link-entity>
    </link-entity>
    <link-entity name='contractdetail' from='contractdetailid' to='dbc_contractlineid' visible='false' link-type='outer' alias='contractdetails'>
      <attribute name='activeon' />
      <attribute name='productid' />
      <attribute name='expireson' />
      <attribute name='title' />
    <link-entity name='contract' from='contractid' to='contractid' alias='aa'>
        <filter type = 'and' >
          <condition attribute='statecode' operator='in'>
            <value>2</value>
            <value>1</value>
          </condition>
        </filter>
      </link-entity>
    </link-entity>
  </entity>
</fetch>";

        #endregion

        #region Below is the Fetch XML used for GetContracts Method 

        public static string FetchXmlForGetContracts = @"<?xml version='1.0'?>

<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
  <entity name='dbc_amcontractline'>
    <attribute name='dbc_supportedbyid' />
    <attribute name='dbc_customerid' />
    <attribute name='dbc_amcontractlineid' />
    <order attribute='dbc_customerid' descending='false' />
    <filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    <link-entity name='account' from='accountid' to='dbc_customerid' alias='ay'>
      <filter type='and'>
        <condition attribute='statecode' operator='eq' value='0' />
      </filter>
    </link-entity>
    <link-entity name='contractdetail' from='contractdetailid' to='dbc_contractlineid' visible='false' link-type='outer' alias='contractdetails'>
      <attribute name='activeon' />
      <attribute name='productid' />
      <attribute name='expireson' />
      <attribute name='title' />
<link-entity name='contract' from='contractid' to='contractid' alias='aa'>
        <filter type = 'and' >
          <condition attribute='statecode' operator='in'>
            <value>2</value>
            <value>1</value>
         </condition>
        </filter>
      </link-entity>
    </link-entity>
  </entity>
</fetch>";

        #endregion
    }
}
