namespace Azure.Services.Interaction.B2C.Model
{
    using System;

    /// <summary>
    /// Object Active Directory B2C User created.
    /// </summary>
    public class CreateAdB2CUser
    {
        public string objectType { get; set; } = "User";
        public string objectId { get; set; }
        public bool accountEnabled { get; set; } = true;
        public string city { get; set; }
        public object country { get; set; }
        public DateTime createdDateTime { get; set; }
        public string creationType { get; set; } = "LocalAccount";
        public string displayName { get; set; }
        public string jobTitle { get; set; }
        public string mailNickname { get; set; }
        public object mobile { get; set; }
        public string passwordPolicies { get; set; } = "DisablePasswordExpiration";
        public Signinname[] signInNames { get; set; }
        public object state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }
        public string userPrincipalName { get; set; }
        public string userType { get; set; } = "Member";
        public PasswordProfile passwordProfile { get; set; }
    }
}
