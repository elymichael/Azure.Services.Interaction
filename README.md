# Azure.Services.Interaction
 
 #### Technology: 
 - .Net Core 5.0.
 #### Prerequisites: 
 - Access to Azure Services.
 - Visual Studio 2019 or greater.
 
 ## Reference table
 
 - [Description](#Description)
 - [Implementation](#Implementation)
 - [Azure Configuration](#Azure-Configuration)
 - [Testing](#Testing)


 ## Description
 This project will help us to interact with some Azure functions more easy. The idea is allow to the user interact quickly to the users with the following functionalities:
 
 - [x] Azure Service Storage
 - [x] Azure Service Bus
 - [x] Azure B2C Active Directory

## Implementation

To implement the 3 services defined in this solution we need to follow the next steps:

#### Service Bus implementation
Add the reference to your API project, after that, open the startup file and include the service registration.
```cs
// Add Azure Service Bus Service.
services.AddAzureServiceBusServices(Configuration);          
```
Json Settings.
```json
"ServiceBus": {
 "ConnectionString": "Endpoint=sb://127.0.0.1:10005//;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxx",
 "QueueName": "main_queue"
},
```
Now, you are able to use the service.

#### Service Storage implementation
Add the reference to your API project, after that, open the startup file and include the service registration. For table storage we are passing the domain class derived of the TableEntity owner with the structure to save.
```cs
// Add Azure Storage Blob and Table Service.
services.AddAzureStorageServices<ItemTransaction>(Configuration);
```
App Settings configuration using connnection string.
```json
"AzureServiceStorage": {
    "ConnectionString": "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=XxxxxXXXdfXxxxXsxxxmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6xx/K1SZFPTOtr/KBHBeksoXXXw==;"
},
```
#### Service B2C implementation
Add the reference to your API project, after that, open the startup file and include the service registration.
```cs
// Add Azure B2C Service.
services.AddAzureB2CServices(Configuration);
```

Add the following configuration to your appSettings.json file and change the data with your Azure B2C configuration.
```json
"AzureB2CClient": {
    "TenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "ClientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "ClientSecret": "xxXxwxxbxxxx_xxxxxxxx_~xxxxxx-xxxx",
    "TenantLoginUrl": "https://login.microsoftonline.com/",
    "GraphResourceUrl": "https://graph.windows.net/",
    "GraphVersion": "api-version=1.6",
    "Instance": "https://ajuala.b2clogin.com",
    "Domain": "ajuala.onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1_JustSignIn",
    "AllowWebApiToBeAuthorizedByACL": true
},
```
## Azure Configuration

To find the configuration used in our implementation, find the information provided below when you configure  your Azure Service. This information will provide you the functionalities required to configure our services.

### Azure Service Bus Configuration

This service works like a tipical Message Queue when you can push some messages and pop for the service bus handle. If you want to see the message bus in your terminal, you can download the following <a href='https://github.com/paolosalvatori/ServiceBusExplorer'>tool</a> to follow the messages.

- [ ] **Queue NAME:** main_queue
- [ ] **Queue URL:** http://127.0.0.1:10005/dev_dlq_queue
- [ ] **Primary Key:** [##SharedAccessKey##]
- [ ] **Primary Conn String:** Endpoint=sb:http://127.0.0.1:10005/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[##SharedAccessKey##]

### Azure Service Storage Configuration

This configuration works for Storage and Table, the simplest option is just copy the connectionstring returned when you finished the configuration of the Azure Service Storage. If you want to see the message transactions just download the **Microsoft Azure Storage**.

- [ ] **Conn string:** BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1/;FileEndpoint=http://127.0.0.1:10000/devstoreaccount1/;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1/;SharedAccessSignature=[SharedAccessSignature]
- [ ] **Table service SAS URL:** http://127.0.0.1:10002/devstoreaccount1/?[##SharedAccessSignature##]
- [ ] **Queue service SAS url:** http://127.0.0.1:10001/devstoreaccount1/?[##SharedAccessSignature##]
- [ ] **SAS token:** http://127.0.0.1:10001/devstoreaccount1/?[##SharedAccessSignature##]

### Azure Service B2C Configuration

This is the configuration you get when you configure the Azure B2C service to integrate the Azure Active Directory with our API.

- [ ] **Instance:** "https://xxxxxxx.b2clogin.com"
- [ ] **ClientId:** "xxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx"
- [ ] **Domain:** "xxxxxxx.onmicrosoft.com"
- [ ] **SignUpSignInPolicyId:** "B2C_1_JustSignIn"

## Testing
For testing purpose, we included a testing project <a href='https://github.com/elymichael/Azure.Services.Interaction/tree/main/Azure.Services.Interaction.API'> Azure.Services.Integration.API</a> to see how to implement the different libraries in your .net core API project. These libraries can be execute in API or any other type of application. Para realizar las pruebas, descargue el proyecto e indique al proyecto **Azure.Services.Interaction.API** como "Set as startup project" and the swagger has some default configuration for testing purpose.

![sample](https://github.com/elymichael/Azure.Services.Interaction/blob/main/Azure.Services.Interaction.API/Img/Sample-01.png)
