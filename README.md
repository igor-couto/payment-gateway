# Payment Gateway

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://github.com/igor-couto/payment-gateway/blob/main/LICENSE)
[![Build Badge](https://github.com/igor-couto/payment-gateway/actions/workflows/build.yml/badge.svg)](https://github.com/igor-couto/payment-gateway/actions/workflows/build.yml)

An application that allows a merchant to offer a way for their shoppers to pay for their product.

## High Level Design

![](https://github.com/igor-couto/images/blob/main/payment-gateway/payment-gateway%20design.png)

[Payment Gateway](https://en.wikipedia.org/wiki/Payment_gateway)

## How to run the solution

### Using the deployed application

### Running Locally

Step 1
```docker compose up -d```

Step 2

dotnet run --project "Payment Gateway"

Step 3

http://localhost:5127/swagger

Here the application name container is the application you want to run by yoursef. It can be: payment_gateway_api, payment_executor or acquiring_bank_simulator

## About the solution

### Arquiteture

### Request

The request for creating a payment has the following fields:

- **checkoutId (Guid)**: This is the id that represents the checkout process in the purchase made by the shopper with the merchant. It is used as an idempotency key to ensure that this payment will be unique.
- **shopperId (Guid)**: This is the id that represents the shopper (buyer) making the payment.
- **merchantId (Guid)**: This is the id that represents the merchant receiving the payment.
- **amount (string)**: Represents the payment value that should be charged for the transaction, expressed in the format "00.00". It should be noted that this field should only contain numerical values and the decimal point, with no other characters such as commas or currency symbols.
- **currency (string)**: Currency used in this payment. It is represented in three letters alphabetic code using the ISO 4217 format.
- **creditCard (string)**: Information about the credit card used for payment.
    - **creditCard.holder**: The name of the credit card owner, usually printed on the front of the card.
    - **creditCard.cardNumber**: A credit card number is the long set of digits that usually appear on the front or back of your credit card. It is used to identify your credit card account.
    - **creditCard.cardVerificationValue**: The Card Verification Value (CVV), also known as Card Security Code (CSC) is a number that is usually printed on the back of a credit card. The code is a security feature that allows the credit card processor to identify the cardholder.
    - **creditCardExpirityMonth**: Credit card expiration month.
    - **creditCardExpirityYear**: Credit card expiration year.


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

## Areas for improvement
TODO

## Cloud technologies

For this project, the chosen cloud solution was Amazon AWS.
Several services were used for development:

- Identity and Access Management (IAM): Creating a new user, grant permissions to use AWS services 
- AWS Cloudwatch: Stream the application logs 
- Amazon Simple Queue Service (SQS): 
- Amazon API Gateway
- AWS ECS
- AWS Secrets Manager for password management and rotation
- AWS RDS running Postgres

## Author

Feel free to get in touch with me regarding any questions or issues about the Payment Gateway solution

* **Igor Couto** - [igor.fcouto@gmail.com](mailto:igor.fcouto@gmail.com)
