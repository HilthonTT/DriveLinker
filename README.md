# DriveLinker

> **⚠️ This project has a successor: [Helix](https://github.com/HilthonTT/Helix)**
>
> DriveLinker is no longer actively developed. Development continues in **Helix**, which supersedes this project. New users should start there; existing users are encouraged to migrate.

DriveLinker is a user-friendly GUI application built with C# .NET MAUI that allows you to connect network drives on Windows using the `net use` command. It provides an encrypted local store for your drive credentials, supports multiple languages, and includes features like an auto-minimize timer and account recovery keys.

## Features

- Connect network drives using the `net use` command with an intuitive graphical interface.
- Drive credentials (username, IP address and password) are encrypted with **AES** before being written to disk.
- Data is stored in a SQLCipher-encrypted SQLite database (SQLite-net-pcl); the database password is randomly generated and held in the platform's `SecureStorage`.
- Your app login password is never stored in plain text — it is verified against a **SHA-512** hash kept in `SecureStorage`.
- Multi-language support including English, French, German, and Indonesian.
- Manage multiple accounts for easy access to various network drives.
- Update the `net use` command directly from the drive's page in the app.
- Customize the timer duration for app minimization in the settings.
- Generate RecoveryKeys so you can reset your login password if you forget it.

## Security notes

DriveLinker was built as a personal convenience tool, not as a hardened credential vault. Be aware of the following before trusting it with sensitive credentials:

- **Drive passwords are recoverable by design.** The app has to hand the plain-text password to `net use`, so drive credentials are encrypted (AES) rather than hashed. Anyone with access to your unlocked Windows user session can read them back.
- **The AES key is stored alongside the ciphertext.** Each drive row persists its own `Key` and `Iv` next to the encrypted fields, so the AES layer offers no protection beyond the SQLCipher database password itself.
- **The login password hash is unsalted and single-round.** SHA-512 without a salt or key-stretching (PBKDF2/Argon2) offers weak protection against offline attacks.
- **RecoveryKeys use `System.Random`.** They are not generated with a cryptographically secure RNG and should not be treated as high-entropy secrets.
- All protection ultimately rests on the OS-level `SecureStorage` and your Windows account security.

## Installation

To run DriveLinker, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/HilthonTT/DriveLinker.git
   ```
2. Open the project in a C# .NET MAUI development environment.
3. Build and run the app on your Windows device.

## Usage

1. Launch the DriveLinker app on your Windows device.
2. Add your network drive accounts, providing the necessary details and passwords.
3. Connect to your network drives using the intuitive GUI.
4. If you need to update the `net use` command for any drive, navigate to the respective drive's page and modify it.
5. Customize the timer duration for minimizing the app in the settings if needed.
6. Store your RecoveryKeys somewhere safe so you can reset your login password if you forget it.

## Contribution

DriveLinker is in maintenance mode — new features are being built in [Helix](https://github.com/HilthonTT/Helix). Bug fixes and small improvements are still welcome here.

To contribute, follow these steps:

1. Fork the repository.
2. Create a new branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Make your changes and commit them:
   ```bash
   git commit -m 'Add some feature'
   ```
4. Push to the branch:
   ```bash
   git push origin feature/your-feature-name
   ```
5. Create a pull request.

Please ensure that your code follows our coding guidelines and maintains a clean commit history.

## License

DriveLinker is licensed under the [MIT License](LICENSE).

---

Happy driving with DriveLinker!
