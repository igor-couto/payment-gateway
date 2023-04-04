# Payment Gateway

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://github.com/igor-couto/payment-gateway/blob/main/LICENSE)
[![Build Badge](https://github.com/igor-couto/payment-gateway/actions/workflows/build.yml/badge.svg)](https://github.com/igor-couto/payment-gateway/actions/workflows/build.yml)

An application that allows a merchant to offer a way for their shoppers to pay for their product.

After researching the role of a payment gateway and its multiple functionalities, I decided to make my reduced version. There is no prior credit card validity check or fraud analysis. The acquiring bank simulator only accepts an authorization request and then a capture request to finalize the transaction.

## High Level Design

![](https://github.com/igor-couto/images/blob/main/payment-gateway/payment-gateway%20design.png)

In this solution, the following systems were developed:

**Payment Gateway API**:

It is the REST API that works as an entry point to the entire system. Receive, validate and register payment requests. It also can retrieve previously recorded payments.

**Queues**: I have chosen to use message broker queues to enhance overall efficiency, reliability, and fault tolerance. By incorporating a queue, I can decouple components and also promote scalability, allowing the entire system to handle increased workloads with ease by distributing messages among multiple instances of consumers. Furthermore, the asynchronous processing capabilities provided by queues help prevent bottlenecks, ensuring smooth operation.

**Database**: The database will store information regarding the payment and its status. A traditional relational database is ideal for this type of system. PostgreSQL was chosen because it is a powerful, open-source, object-relational database with ACID compliance.

**Payment Executor**: 

It's a worker type application that runs constantly looking for messages in the queue. It was done that way for practicality but in the future it could be transformed into an AWS Lambda Function that would listen to queue events.

**Acquiring Bank Simulator**:

It's a small and simple minimal .NET API that only receives requests and returns programmed responses. It was built to be simple and easy to change, without too much attention to technical details. There are two endpoints available:
> POST authorize
> 
> POST authorize/{authorizationId}/capture

## How to run the solution

### Prerequisites

Make sure you have the following components installed:
- [.NET 7 Runtime and SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Docker](https://www.docker.com/products/docker-desktop/)

For applications to run successfully, you need the following ports available
- PosPostgreSQL Database: 4566
- AWS LocalStack: 5432
- Acquiring Bank Simulator: 5087
- Payment Gateway API: 5127 and 7290

### Running Locally

**Step 1**

In your terminal, navigate to the root of the project and run the following commands:

```bash
docker compose up -d
```
The following containers will be started: _payment_database_, _localstack_, _ acquiring_bank_simulator_ and _payment_executor_

**Step 2**

Still in your terminal and in the root of the project, run:

```bash
dotnet run --project "Payment Gateway"
```

...or if you prefer open the `Payment Gateway.sln` solution in your preferred IDE and select RUN. Make sure the project that will run will be the Payment Gateway API.

**Step 3**

Open `http://localhost:5127/swagger` to access the Swagger UI where you can interact with the Payment Gateway web API.

![](https://github.com/igor-couto/images/blob/main/payment-gateway/payment-gateway-api-swagger.png)

If you prefer, make http requests to `http://localhost:5127` using a client like [Postman](https://www.postman.com/downloads/) or [Insomnia](https://insomnia.rest/download).

## About the solution

### Arquiteture
//TODO

### Cloud technologies

For this project, the chosen cloud solution was Amazon AWS.
Several services were used for development:

- Identity and Access Management (IAM): Creating a new user, grant permissions to use AWS services 
- AWS Cloudwatch: Stream the application logs 
- Amazon Simple Queue Service (SQS): 
- AWS RDS running Postgres

### Request

The request for creating a payment has the following fields:

- **checkoutId (Guid)**: This is the id that represents the checkout process in the purchase made by the shopper with the merchant. It is used as an idempotency key to ensure that this payment will be unique.
- **shopperId (Guid)**: This is the id that represents the shopper (buyer) making the payment.
- **merchantId (Guid)**: This is the id that represents the merchant receiving the payment.
- **amount (string)**: Represents the payment value that should be charged for the transaction, expressed in the format "00.00". It should be noted that this field should only contain numerical values and the decimal point, with no other characters such as commas or currency symbols.
- **currency (string)**: Currency used in this payment. It is represented in three letters alphabetic code using the ISO 4217 format.
- **creditCard (string)**: Information about the credit card used for payment.
    - **holder**: The name of the credit card owner, usually printed on the front of the card.
    - **cardNumber**: A credit card number is the long set of digits that usually appear on the front or back of your credit card. It is used to identify your credit card account.
    - **cardVerificationValue**: The Card Verification Value (CVV), also known as Card Security Code (CSC) is a number that is usually printed on the back of a credit card. The code is a security feature that allows the credit card processor to identify the cardholder.
    - **expirityMonth**: Credit card expiration month.
    - **expirityYear**: Credit card expiration year.


Exemple:

```json
{
  "checkoutId": "58d0d874-d7b6-47f1-83cb-c439735b9638",
  "shopperId": "43641647-f1cd-4187-b945-89f53e168bc1",
  "merchantId": "480a6e61-25c1-4cec-818e-157ad9ade682",
  "amount": "12.50",
  "currency": "USD",
  "creditCard": {
    "holder": "Igor Freitas Couto",
    "cardNumber": "4980-5730-7604-795",
    "cardVerificationValue ": "121",
    "ExpirityMonth": 7,
    "ExpirityYear": 2023
  }
}
```

## Assumptions and considerations
TODO

| Credit Card Number  | Problem                    | Message                                     |
| ------------------- | -------------------------- | ------------------------------------------- |
| 5385-6109-7070-6057 | XXXXXXXXXXXX               | XXXXXXXXXXXX                                |
| 5197-6066-7512-2317 | XXXXXXXXXXXX               | XXXXXXXXXXXX                                |
| 5144-8846-1008-0494 | XXXXXXXXXXXX               | XXXXXXXXXXXX                                |
| 3479-3027-3551-4362 | XXXXXXXXXXXX               | XXXXXXXXXXXX                                |

## Areas for improvement
- Write more unit teste to achieve a better coverage
- Write integration and functional tests
- Add redis or dotnet in memory cache to perform the search for repeated payments by it's checkout id (idempotency)
- Write a pipeline to deploy the application in EC2
- Configure a Dead Letter Queue in AWS LocalStack. In the moment, only the AWS Production environment have a Dead Letter Queue. 

## Author

Feel free to get in touch with me regarding any questions or issues about the Payment Gateway solution

* **Igor Couto** - [igor.fcouto@gmail.com](mailto:igor.fcouto@gmail.com)
