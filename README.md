# 💼📱 MiniDigitalWalletApi

This project demonstrates the implementation of a digital wallet service using ASP.NET Core. It encompasses user registration, profile updates, pin changes, fund transfers, withdrawals, deposits, and transaction history retrieval with pagination. The service and controller are designed to ensure robust validation and efficient handling of user data.

## 💳🛠️ Wallet User Features

1. **Register User**: 
   - **Service**: Validates and registers a new user by ensuring all required fields such as mobile number, username, and pin code are correctly provided and formatted. The pin code must be exactly 6 characters long.
   - **Controller (HttpPost)**: Receives user registration requests, calls the service to handle the logic, and returns the created user or validation errors.

2. **Update Profile**:
   - **Service**: Allows users to update their profile details including username, mobile number, balance, and status. Each field undergoes validation to ensure data integrity.
   - **Controller (HttpPatch)**: Processes requests for updating user profiles, leveraging the service to perform necessary validations and updates.

3. **Change Pin**:
   - **Service**: Provides functionality for users to change their pin code. The new pin code is validated to be exactly 6 characters.
   - **Controller (HttpPatch)**: Handles pin change requests, ensuring the new pin meets the required criteria.

4. **Transfer Funds**:
   - **Service**: Handles the logic for transferring funds between users. Ensures the sender has sufficient balance and updates both sender's and receiver's balances accordingly. A transaction record is created for each transfer.
   - **Controller (HttpPost)**: Manages fund transfer requests between users, validating and executing the transfer through the service.

5. **Withdraw Funds**:
   - **Service**: Handles the logic for withdrawing funds from a user's account, ensuring they have sufficient balance. A transaction record is created for each withdrawal.
   - **Controller (HttpPost)**: Manages withdrawal requests, validates and processes the withdrawal, and records the transaction.

6. **Deposit Funds**:
   - **Service**: Handles the logic for depositing funds into a user's account. A transaction record is created for each deposit.
   - **Controller (HttpPost)**: Manages deposit requests, validates and processes the deposit, and records the transaction.

7. **Transaction History with Pagination**:
   - **Service**: Retrieves the transaction history for a user, supporting pagination to manage large data sets effectively. Transactions are ordered by date, with the most recent first.
   - **Controller (HttpGet)**: Provides paginated transaction history for a user, making it easier to navigate through extensive transaction records.

```
dotnet ef dbcontext scaffold "Server=.;Database=MiniDigitalWalletDb;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```

```sql
-- Schema for table: Tbl_WalletUsers
CREATE TABLE Tbl_WalletUsers (UserId int NOT NULL, MobileNumber varchar NOT NULL, UserName varchar NOT NULL, PinCode varchar NOT NULL, Balance decimal NULL DEFAULT ((0.00)), Status varchar NULL DEFAULT ('active'));
INSERT INTO Tbl_WalletUsers (UserId, MobileNumber, UserName, PinCode, Balance, Status) VALUES ('7', '1234567890', 'Alice', '123456', '1000.00', 'active');
INSERT INTO Tbl_WalletUsers (UserId, MobileNumber, UserName, PinCode, Balance, Status) VALUES ('8', '0987654321', 'Bob', '654321', '500.00', 'active');
INSERT INTO Tbl_WalletUsers (UserId, MobileNumber, UserName, PinCode, Balance, Status) VALUES ('9', '1122334455', 'Charlie', '112233', '750.50', 'suspended');
INSERT INTO Tbl_WalletUsers (UserId, MobileNumber, UserName, PinCode, Balance, Status) VALUES ('10', '5566778899', 'David', '445566', '1200.00', 'active');
INSERT INTO Tbl_WalletUsers (UserId, MobileNumber, UserName, PinCode, Balance, Status) VALUES ('11', '6677889900', 'Eva', '778899', '300.75', 'suspended');

GO

-- Schema for table: Tbl_Transactions
CREATE TABLE Tbl_Transactions (TransactionId int NOT NULL, SenderUserId int NULL, ReceiverUserId int NOT NULL, TransactionType varchar NULL, Amount decimal NOT NULL, TransactionDate datetime NOT NULL DEFAULT (getdate()));
INSERT INTO Tbl_Transactions (TransactionId, SenderUserId, ReceiverUserId, TransactionType, Amount, TransactionDate) VALUES ('2', '1', '2', 'transfer', '200.00', '11/19/2024 3:26:13 PM');
INSERT INTO Tbl_Transactions (TransactionId, SenderUserId, ReceiverUserId, TransactionType, Amount, TransactionDate) VALUES ('3', '2', '3', 'transfer', '50.00', '11/19/2024 3:26:13 PM');
INSERT INTO Tbl_Transactions (TransactionId, SenderUserId, ReceiverUserId, TransactionType, Amount, TransactionDate) VALUES ('4', '3', '1', 'transfer', '100.00', '11/19/2024 3:26:13 PM');
INSERT INTO Tbl_Transactions (TransactionId, SenderUserId, ReceiverUserId, TransactionType, Amount, TransactionDate) VALUES ('5', '4', '2', 'transfer', '150.00', '11/19/2024 3:26:13 PM');
INSERT INTO Tbl_Transactions (TransactionId, SenderUserId, ReceiverUserId, TransactionType, Amount, TransactionDate) VALUES ('6', '1', '4', 'transfer', '300.00', '11/19/2024 3:26:13 PM');

GO
```
