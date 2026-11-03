# DriveLinker

[![CI](https://github.com/HilthonTT/DriveLinker/actions/workflows/ci.yml/badge.svg)](https://github.com/HilthonTT/DriveLinker/actions/workflows/ci.yml)
[![Release](https://img.shields.io/github/v/release/HilthonTT/DriveLinker?include_prereleases)](https://github.com/HilthonTT/DriveLinker/releases)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> **⚠️ This project has a successor: [Helix](https://github.com/HilthonTT/Helix)**
>
> DriveLinker is in maintenance mode. New features are built in **Helix**; new users should start there. DriveLinker still receives bug fixes and dependency updates.

DriveLinker is a Windows desktop app (.NET MAUI) for mapping network drives with `net use`. It keeps your share credentials in an encrypted local store, so you can connect or disconnect all your drives with one click. You can also have it link them automatically at startup.

## Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Building from source](#building-from-source)
- [Project layout](#project-layout)
- [Where data is stored](#where-data-is-stored)
- [Security notes](#security-notes)
- [Releasing](#releasing)
- [Contributing](#contributing)
- [License](#license)

## Features

- **One-click linking.** Map or unmap a single drive, or all of them, with `net use`. Connection status and progress are shown live.
- **Auto-link and auto-minimize.** Optionally link every drive when you log in, then minimize the window after a countdown (5–500 s).
- **Start with Windows.** Optionally adds a shortcut to your Startup folder.
- **Multiple local accounts.** Each account has its own drives and settings.
- **Encrypted storage.** Data lives in SQLCipher databases. Drive passwords, IP addresses and usernames are also AES-encrypted, each drive with its own random key and IV.
- **Recovery keys.** Five keys, generated when you register, let you sign in and reset a forgotten password or username.
- **Bulk tools.** Export or import drives as JSON, update the username/password/IP of every drive at once, or delete all drives.
- **Languages:** English, French, German and Indonesian.
- **Error log.** Failed `net use` calls are collected on a stack-trace page.

## Installation

Download the latest build from the [Releases page](https://github.com/HilthonTT/DriveLinker/releases). DriveLinker requires **Windows 10 version 1809 (build 17763) or later**.

| Asset | When to use it |
| --- | --- |
| `DriveLinker-<version>-win-x64-portable.zip` | Most PCs. Unzip anywhere and run `DriveLinker.exe`. Nothing else to install. |
| `DriveLinker-<version>-win-arm64-portable.zip` | Windows on ARM devices. |
| `DriveLinker_<version>.0_x64.msix` + `DriveLinker.cer` | Installable package with Start-menu entry and clean uninstall. Only present when the release was signed. |

To install the MSIX signed with the project's self-signed certificate, trust the certificate once from an elevated PowerShell, then double-click the `.msix`:

```powershell
Import-Certificate -FilePath .\DriveLinker.cer -CertStoreLocation Cert:\LocalMachine\TrustedPeople
```

Verify downloads against `SHA256SUMS.txt`:

```powershell
Get-FileHash .\DriveLinker-2.0.0-win-x64-portable.zip -Algorithm SHA256
```

> The portable build and the MSIX keep their data in different places (see [Where data is stored](#where-data-is-stored)). Pick one and stick with it.

## Usage

1. **Register** a local account, then **save the five recovery keys** that are shown. They are the only way back in if you forget your password.
2. **Create a drive**: enter the drive letter, the server's IP address or host name, the share name, and the share's username and password.
3. On the home page, **Link drives** connects everything. Click a single drive to toggle it, or open it to edit or delete it.
4. In **Settings**, choose the language, auto-link, auto-minimize, the countdown, and whether DriveLinker starts with Windows.
5. Under **Account** you can change your username or password, using either the current password or a recovery key. You can also delete the account.

Each drive is mapped as:

```text
net use <Letter>: \\<IP address>\<Drive name> <password> /user:<username> /persistent:no
```

## Building from source

**Prerequisites**

- Windows 10/11
- [.NET SDK 10.0.400 or later](https://dotnet.microsoft.com/download/dotnet/10.0) (pinned in [`global.json`](global.json))
- The .NET MAUI Windows workload:

  ```powershell
  dotnet workload install maui-windows
  ```

- Optional: Visual Studio 2026 with the *.NET Multi-platform App UI development* workload, or VS Code with C# Dev Kit and the .NET MAUI extension.

**Build, test and run**

```powershell
git clone https://github.com/HilthonTT/DriveLinker.git
cd DriveLinker

dotnet build DriveLinker.slnx
dotnet test  --solution DriveLinker.slnx
dotnet run   --project DriveLinker/DriveLinker.csproj -p:WindowsPackageType=None   # unpackaged
```

**Produce the release artifacts locally**

```powershell
# Portable, self-contained folder
dotnet publish DriveLinker/DriveLinker.csproj -c Release -r win-x64 -p:WindowsPackageType=None -o out/portable

# MSIX package (unsigned)
dotnet publish DriveLinker/DriveLinker.csproj -c Release -r win-x64 -p:WindowsPackageType=MSIX `
  -p:GenerateAppxPackageOnBuild=true -p:AppxPackageSigningEnabled=false
```

Local **Release** builds sign the MSIX with the maintainer's certificate, looked up by thumbprint in `DriveLinker.csproj`. If that certificate isn't in your store, pass `-p:AppxPackageSigningEnabled=false` or build in Debug.

## Project layout

```text
DriveLinker.slnx                Solution (XML .slnx format)
global.json                     Pinned .NET SDK and test runner
Directory.Build.props           Settings shared by all projects (CI = warnings as errors)
Directory.Packages.props        Central NuGet package versions
DriveLinker/                    MAUI app: views (XAML), view models, Windows helpers, DI setup
DriveLinker.Core/               Models, SQLite services, encryption, `net use` linker, translations
tests/DriveLinker.Core.Tests/   xUnit tests for the core library
.github/workflows/              CI (every push / PR) and Release (version tags)
```

The app uses MVVM with [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/) source generators and Shell navigation. All services are registered in [`RegisterServices.cs`](DriveLinker/RegisterServices.cs).

## Where data is stored

| Data | Location |
| --- | --- |
| `Account.db4`, `Drive.db4`, `Settings.db4` | `Environment.SpecialFolder.LocalApplicationData`: `%LOCALAPPDATA%` for the portable build, or the package's virtualised `LocalCache` folder for the MSIX |
| Database password, login password hashes, recovery keys | .NET MAUI `SecureStorage`: per-user and protected by Windows |

Deleting an account removes its drives, settings, password hash and recovery keys.

## Security notes

DriveLinker is a personal convenience tool, not a hardened credential vault. Know these limits before trusting it with sensitive credentials:

- **Drive passwords are recoverable by design.** `net use` needs the plain-text password, so drive credentials are encrypted (AES-256-CBC), not hashed. Anyone with access to your unlocked Windows session can read them back.
- **The AES key is stored next to the ciphertext.** Each drive row holds its own random `Key` and `Iv`. The real protection is the SQLCipher database password in `SecureStorage`.
- **Exports are plain text.** *Export drives* writes every drive, **including its password**, unencrypted to `import_drives_file.json`. Delete that file after importing it.
- **The password is visible on the command line.** While `net use` runs, the share password is in its arguments and visible to other processes of the same user.
- **The login password hash is unsalted and single-round** (SHA-512). That is weak against offline attacks if the stored hash leaks.
- **Recovery keys** are 12 characters from a 62-character alphabet, generated with a cryptographically secure RNG.

Everything ultimately rests on Windows account security and `SecureStorage`.

## Releasing

Releases are built by [`.github/workflows/release.yml`](.github/workflows/release.yml):

```powershell
git tag v2.0.0          # or v2.1.0-beta.1 for a pre-release
git push origin v2.0.0
```

The workflow builds and tests the solution, then publishes the portable x64 and arm64 zips. If signing secrets are configured, it also builds a signed MSIX. It writes `SHA256SUMS.txt` and creates the GitHub release with generated notes. You can also start it manually from the Actions tab with a version number.

To ship a signed MSIX, add these repository secrets:

| Secret | Value |
| --- | --- |
| `SIGNING_CERTIFICATE_BASE64` | The `.pfx` code-signing certificate, Base64-encoded: `[Convert]::ToBase64String([IO.File]::ReadAllBytes('cert.pfx'))` |
| `SIGNING_CERTIFICATE_PASSWORD` | The `.pfx` password |

The certificate subject must match `Publisher` in [`Package.appxmanifest`](DriveLinker/Platforms/Windows/Package.appxmanifest) (currently `CN=Timothy Tandt`). The workflow checks this and fails early on a mismatch.

Dependency updates arrive as grouped Dependabot pull requests every week ([`.github/dependabot.yml`](.github/dependabot.yml)).

## Contributing

Bug fixes and small improvements are welcome. New features belong in [Helix](https://github.com/HilthonTT/Helix).

1. Fork the repository and create a branch: `git checkout -b fix/short-description`.
2. Make your change and add or update tests where possible.
3. Make sure `dotnet build DriveLinker.slnx -c Release -p:CI=true` (warnings are errors) and `dotnet test --solution DriveLinker.slnx` pass.
4. Open a pull request describing the problem and the fix. CI must be green.

Code style is defined in [`.editorconfig`](.editorconfig). Package versions live in [`Directory.Packages.props`](Directory.Packages.props); don't put `Version` attributes on `PackageReference`s.

## License

DriveLinker is licensed under the [MIT License](LICENSE).
