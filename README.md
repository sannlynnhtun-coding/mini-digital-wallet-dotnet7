# 💼📱 MiniDigitalWalletApi

This project demonstrates the implementation of a digital wallet service using ASP.NET Core. It encompasses user registration, profile updates, pin changes, fund transfers, and transaction history retrieval with pagination. The service and controller are designed to ensure robust validation and efficient handling of user data.

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

5. **Transaction History with Pagination**:
   - **Service**: Retrieves the transaction history for a user, supporting pagination to manage large data sets effectively. Transactions are ordered by date, with the most recent first.
   - **Controller (HttpGet)**: Provides paginated transaction history for a user, making it easier to navigate through extensive transaction records.

This structure ensures a clean separation of concerns, with the service layer responsible for business logic and the controller handling HTTP requests and responses. The domain logic prioritizes data validation and efficient handling of user operations to deliver a seamless digital wallet experience. 🚀🔐✨
