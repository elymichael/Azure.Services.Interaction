using Azure.Services.Interaction.B2C.Commands;
using Azure.Services.Interaction.B2C.Model;
using System.Collections.Generic;

namespace Azure.Services.Interaction.B2C.Test.Helper
{
    public static class CommandsHelper
    {
        public static UserCredentialCommand FakeCredentialCommand()
        {
            return new UserCredentialCommand()
            {
                NewPassword = "Admin_deF78#"
            };
        }

        public static CreateUserInfoCommand FakeAddUserInfoCommand()
        {
            return new CreateUserInfoCommand()
            {
                Name = "Ely",
                LastName = "Nunez",
                CustomerId = "5",
                EmailAddress = "ely.nunez@truenorth.co",
                Password = "adminChange#2342@"
            };
        }

        public static AzureB2CAdUserRoot GetB2CUserMockData()
        {
            return new AzureB2CAdUserRoot()
            {
                value = new B2CADUser[2] {
                    new B2CADUser {
                        objectId = "fda4f8a7f8af",
                        jobTitle = "1",
                        displayName = "ely",
                        accountEnabled = false,
                        signInNames = new Signinname[] { new Signinname { type = "emailAddress", value = "ely.nunez@truenorth.co" } }
                    },
                    new B2CADUser {
                        objectId = "454fd87adf8",
                        jobTitle = "6",
                        displayName = "Adrian",
                        accountEnabled = true,
                        signInNames = new Signinname[] { new Signinname { type = "emailAddress", value = "adrian.fungueirino@truenorth.co" } }
                    }
                }
            };
        }

        public static List<AzureB2CAdUserSearchResult> GetMockUserSearchData()
        {
            return new List<AzureB2CAdUserSearchResult>()
            {
                new AzureB2CAdUserSearchResult
                {
                    ObjectId = "584fd87adf8",
                     DisplayName = "Ely",
                        jobTitle = "1",
                        AccountEnabled = false,
                        emailAddress = "ely.nunez@truenorth.co"
                },
                new AzureB2CAdUserSearchResult {
                    ObjectId = "454fd87adf8",
                    jobTitle = "6",
                    DisplayName = "Adrian",
                    AccountEnabled = true,
                    emailAddress =  "adrian.fungueirino@truenorth.co"
                }
            };
        }
    }
}
