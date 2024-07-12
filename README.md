Wallet Service

The Wallet Service application is a simple microservice developed in .NET 5, designed to manage user wallets in a sports betting platform.
This service provides essential functionality for creating wallets, adding funds, removing funds, and querying the wallet balance. The service interacts with clients through REST APIs and ensures data integrity and consistency by implementing concurrency control and transaction logging mechanisms.

It is a DB-less application which is now saving data momentarily in Memory. But can eventually be attached to a database.
Also, the API does not implement authentication. 

The service allows us to firstly create a ‘Wallet’, consisting of a GUID. Eventually, a wallet will be linked to a specific user and we will be able to use the username to retrieve data. But for now, we’ll limit ourselves to creating the Wallet.
Once the wallet is created, we can use the GUID generated to perform other CRUD operations, such as adding or consuming the funds.
Some validations have been implemented, such as preventing the consumption of an amount larger than the existing fund amount, or adding a negative value to the fund. Also, users cannot spend same amount of funds twice, within a time limit (set as 5 minutes, for now).
Interaction to the application will be done via REST API, and will be tested using Postman. Below is a set of test cases performed. 
Key Features 

Create Wallet:
Allows the creation of a new wallet for a user.
Add Funds: 
Enables adding funds to a user's wallet.
Remove Funds: 
Supports removing funds from a user's wallet with concurrency control to prevent double-spending.
Query Balance: 
Retrieves the current balance of a user's wallet.
Concurrency Control:
Prevents the same user from removing the same amount twice by implementing versioning and concurrency tokens.
Transaction Logging:
Logs all transactions (add and remove operations) for auditing and tracking purposes.
A log file is created at a path that is configurable in the appsettings.json file.
Configuration Management:
Uses appsettings.json for managing application settings and log configurations.

Potential Improvements

Enhanced Security:
Authentication and Authorization: Implement OAuth2 or JWT-based authentication to ensure that only authorized users can access and manipulate wallet data.

Scalability and Performance:
Caching: Implement caching strategies to reduce database load and improve response times for frequently accessed data.
Load Balancing: Deploy the service behind a load balancer to distribute traffic evenly across multiple instances, enhancing availability and fault tolerance.

Advanced Transaction Management:
Transaction History API: 
Provide endpoints to query the transaction history of a wallet, offering detailed insights into all operations performed.
Retry Mechanism: 
Implement a retry mechanism for failed transactions to handle transient errors gracefully.

Monitoring and Logging:
Centralized Logging:
Integrate with centralized logging systems (e.g., ELK stack or Azure Monitor) for better log management and analysis.

Testing and Quality Assurance:
Unit and Integration Tests: Enhance test coverage by adding comprehensive unit and integration tests to ensure the reliability of the service.
Continuous Integration/Continuous Deployment (CI/CD): Set up CI/CD pipelines to automate testing, building, and deployment processes, ensuring rapid and reliable delivery of updates.

User Experience and API Documentation:
Swagger/OpenAPI: 
Integrate Swagger or OpenAPI to provide interactive API documentation, making it easier for clients to understand and interact with the service.
Rate Limiting: 
Implement rate limiting to protect the service from abuse and ensure fair usage among clients.

By incorporating these improvements, the Wallet Service can become more secure, scalable, and user-friendly, offering a higher level of reliability and performance to meet the demands of a sports betting platform.

 
