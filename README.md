# Azure.Services.Interaction
 
 #### Technology: 
 - .Net Core 5.0.
 #### Prerequisites: 
 - Access to Azure Services.
 - Visual Studio 2019 or greater.
 
 ## Description
 This project will help us to interact with some Azure functions more easy. The idea is allow to the user interact quickly to the users with the following functionalities:
 
 - [x] Azure Service Storage
 - [x] Azure Service Bus
 - [x] Azure B2C Active Directory

## Implementation

To implement the 3 services defined in this solution we need to follow the next steps:

## Configuration

### Azure Service Bus Configuration

This service works like a tipical Message Queue when you can push some messages and pop for the service bus handle. If you want to see the message bus in your terminal, you can download the following <a href='https://github.com/paolosalvatori/ServiceBusExplorer'>tool</a> to follow the messages.

**Queue NAME:** main_queue

**Queue URL:** http://127.0.0.1:10005/dev_dlq_queue

**Primary Key:** [##SharedAccessKey##]

**Primary Conn String:** Endpoint=sb:http://127.0.0.1:10005/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[##SharedAccessKey##]

### Azure Service Storage Configuration

This configuration works for Storage and Table, the simplest option is just copy the connectionstring returned when you finished the configuration of the Azure Service Storage. If you want to see the message transactions just download the **Microsoft Azure Storage**.

**Conn string:** BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1/;FileEndpoint=http://127.0.0.1:10000/devstoreaccount1/;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1/;SharedAccessSignature=[SharedAccessSignature]

**Table service SAS URL:** http://127.0.0.1:10002/devstoreaccount1/?[##SharedAccessSignature##]

**Queue service SAS url:** http://127.0.0.1:10001/devstoreaccount1/?[##SharedAccessSignature##]

**SAS token:** http://127.0.0.1:10001/devstoreaccount1/?[##SharedAccessSignature##]

### Azure Service B2C Configuration

This is the configuration you get when you configure the Azure B2C service to integrate the Azure Active Directory with our API.

**Instance:** "https://xxxxxxx.b2clogin.com"

**ClientId:** "xxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx"

**Domain:** "xxxxxxx.onmicrosoft.com"

**SignUpSignInPolicyId:** "B2C_1_JustSignIn"
