namespace Azure.Services.Interaction.B2C.Model
{
    using System;

    /// <summary>
    /// Azure B2C Ad User Result.
    /// </summary>
    public class AzureB2CAdUserResult
    {
        public string odatametadata { get; set; }
        public string odatatype { get; set; }
        public string objectType { get; set; }
        public string objectId { get; set; }
        public object deletionTimestamp { get; set; }
        public bool accountEnabled { get; set; }
        public object ageGroup { get; set; }
        public object[] assignedLicenses { get; set; }
        public object[] assignedPlans { get; set; }
        public object city { get; set; }
        public object companyName { get; set; }
        public object consentProvidedForMinor { get; set; }
        public object country { get; set; }
        public object createdDateTime { get; set; }
        public string creationType { get; set; }
        public object department { get; set; }
        public object dirSyncEnabled { get; set; }
        public string displayName { get; set; }
        public string employeeId { get; set; }
        public object facsimileTelephoneNumber { get; set; }
        public object givenName { get; set; }
        public object immutableId { get; set; }
        public object isCompromised { get; set; }
        public string jobTitle { get; set; }
        public object lastDirSyncTime { get; set; }
        public object legalAgeGroupClassification { get; set; }
        public object mail { get; set; }
        public string mailNickname { get; set; }
        public object mobile { get; set; }
        public object onPremisesDistinguishedName { get; set; }
        public object onPremisesSecurityIdentifier { get; set; }
        public object[] otherMails { get; set; }
        public string passwordPolicies { get; set; }
        public object passwordProfile { get; set; }
        public object physicalDeliveryOfficeName { get; set; }
        public object postalCode { get; set; }
        public object preferredLanguage { get; set; }
        public object[] provisionedPlans { get; set; }
        public object[] provisioningErrors { get; set; }
        public object[] proxyAddresses { get; set; }
        public DateTime refreshTokensValidFromDateTime { get; set; }
        public object showInAddressList { get; set; }
        public Signinname[] signInNames { get; set; }
        public object sipProxyAddress { get; set; }
        public object state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }
        public string thumbnailPhotoodatamediaEditLink { get; set; }
        public object usageLocation { get; set; }
        public object[] userIdentities { get; set; }
        public string userPrincipalName { get; set; }
        public object userState { get; set; }
        public object userStateChangedOn { get; set; }
        public string userType { get; set; }
    }
}
