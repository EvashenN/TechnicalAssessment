# Step 2  - Web API

This branch contains a solution for the C# REST API based on the game of roulette.

## Bets and Payouts

This API supports the following bets:

-   **Low/High:**  1 to 1
-   **Red/Black:**  1 to 1
-    **Odd/Even:**  1 to 1
-   **Dozens:**  2 to 1
-   **Columns:**  2 to 1
-    **Straight:**  35 to 1


## Examples

 - **Place bet:**

 **POST**  `api/Roulette/bet`

**Sample Request:**

    {
      "type": "straight",
      "number": 23,
      "amount": 100
    }

 - **Return active bets:**

 **GET**  `api/Roulette/bet`
 

 - **Return bet:**

 **GET**  `api/Roulette/bet/1`
 
 

 - **Spin:**

 **GET**  `api/Roulette/spin`
 
 

 - **View spins in current play:**

 **GET**  `api/Roulette/previousSpins`
 
 

 - **Payout winnings:**

 **GET**  `api/Roulette/payout`
 



## Features

 - C# .NET  5.0 
 - SQLite DB and Dapper
 - Repository Design Pattern
 - Global exception handling with the built-in middleware
 - Custom logging using NLog
 

## Development

Version  **1.0.0**

Written by  **Evashen Naicker**
