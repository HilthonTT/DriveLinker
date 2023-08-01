# DriveLinker
DriveLinker is a user-friendly GUI application built with C# .NET Maui that allows you to connect network drives on Windows using the net use command. It provides a secure and encrypted way to store your account information and passwords, supporting multiple languages and features like timers and recovery keys.

Features
Connect network drives using the net use command with an intuitive graphical interface.
Securely store your account passwords using SHA512 encryption.
Utilize SQLite-net-pcl to store your data with encryption, and the password for the database is secured using SecureStorage.
Multi-language support including English, French, German, and Indonesian.
Ability to manage multiple accounts for easy access to various network drives.
Update the net use command directly from the drive's page in the app.
Customize the timer duration for app minimization in the settings.
Backup and recover your passwords using RecoveryKeys in case of lost credentials.
Installation
To run DriveLinker, follow these steps:

Clone the repository: git clone https://github.com/HilthonTT/DriveLinker.git
Open the project in C# .NET Maui development environment.
Build and run the app on your Windows device.
Usage
Launch the DriveLinker app on your Windows device.
Add your network drive accounts, providing the necessary details and passwords.
Connect to your network drives using the intuitive GUI.
In case you need to update the net use command for any drive, navigate to the respective drive's page and modify it.
Customize the timer duration for minimizing the app in the settings if needed.
Make sure to securely store your RecoveryKeys in case you forget your passwords.
Contribution
We welcome contributions to DriveLinker. To contribute, follow these steps:

Fork the repository.
Create a new branch: git checkout -b feature/your-feature-name.
Make your changes and commit them: git commit -m 'Add some feature'.
Push to the branch: git push origin feature/your-feature-name.
Create a pull request.
Please ensure that your code follows our coding guidelines and maintain a clean commit history.

License
DriveLinker is licensed under the MIT License.

Happy driving with DriveLinker!
