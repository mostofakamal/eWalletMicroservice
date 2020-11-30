# Introduction 

This project is an implementation of Microservice architecture of e-Wallet project.  It demonstrates how to design Bounded context , communication among different microservices with Integration events , based on a change or action of a specific context how to raise domain events, how to ensure data consistency using Saga pattern,
fault tolerance etc.

It has the following Microservice:

- Identity: Responsible for user authentication and authorizaion
- Kyc: Responsible for managing user KYC (Know your customer ) data
- Transaction: Handles different types of user transaction 
- Reward: Handles and trigger reward transaction for different pre-configured user actions
- Notification: It is responsible for sending out notification e.g. SMS, email using third party provider
- Sagas: It is an implementation of handling long running transactions that spans accross multiple microservices to ensure data integrity and consistency

