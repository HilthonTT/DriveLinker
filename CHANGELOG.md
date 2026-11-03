# Changelog

All notable changes to this project are documented here. The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses [Semantic Versioning](https://semver.org/).

## [Unreleased] - 2.0.0

### Fixed

- **New accounts could not log in until the app was restarted.** Username lookups cached "not found", and registration looks the name up before creating it. Changing your username had the same problem with the new name.
- **Usernames were case-insensitive for lookup but case-sensitive for the password check.** Registering as `Bob` and logging in as `bob` reported a wrong password.
- **Editing a drive replaced its visible credentials with ciphertext.** The drive was encrypted in place, so a later "link" sent the encrypted password to `net use`.
- `AccountService.GetAccountAsync` queried the database with the cache key string instead of the account id.
- Newly generated recovery keys were not used until restart; the old, cached set was still checked.
- Imported drives kept the exporting account's id, so they did not appear for the importing account.
- Passwords, usernames or share names containing spaces broke the `net use` command line.
- `net use /delete` could block waiting for confirmation when files were open on the share.
- The connection progress showed `NaN%` for accounts without drives.
- Loading drive details threw on drives that were not ready (card readers, dropped shares).
- The account page could throw a `NullReferenceException` while translations were still loading.
- The countdown timer updated the UI and minimized the window from a background thread. Auto-minimize also only ran for the first account logged in per session.
- Text was invisible in Windows light mode. The UI is designed for dark mode and is now pinned to it.
- `[JsonIgnore]` on `Drive.Key`, `Iv`, `DriveType` and `DriveFormat` had no effect, so these fields leaked into exports.
- The "username taken" dialog was titled "Error count".
- Deleting an account now also removes its password hash and recovery keys and signs out cleanly.
- Constructors started async work without observing exceptions.

### Security

- Each drive now gets its own random AES key and IV every time it is saved. Previously one IV was shared by every field of every drive created in a session.
- The encryption service is stateless and thread-safe. Previously decrypting a drive overwrote the shared key used for encrypting the next one.
- Recovery keys and the database password use `RandomNumberGenerator` (CSPRNG, no modulo bias) instead of `System.Random`.
- Password hash comparison is constant-time.
- Only the SQLCipher build of sqlite-net is referenced. With both `sqlite-net-pcl` and `sqlite-net-sqlcipher`, the build could pick the non-encrypting SQLite.

### Changed

- Upgraded from .NET 8 to **.NET 10** and .NET MAUI 10. Other updates: CommunityToolkit.Maui 15, CommunityToolkit.Mvvm 8.4 (partial properties), Microsoft.Extensions.* 10.0, SQLitePCLRaw 2.1.11.
- Removed the archived `PInvoke.User32` package and the `IWshRuntimeLibrary` COM reference (replaced by direct P/Invoke and late-bound COM). The app now builds with the `dotnet` CLI.
- `DriveLinker.Core` targets Windows only, like the app. The non-functional macOS code path in the linker was removed.
- Migrated the solution to `DriveLinker.slnx`. Added central package management (`Directory.Packages.props`), `Directory.Build.props`, `global.json`, `.editorconfig` and `.gitattributes`.
- Replaced obsolete MAUI APIs (`Application.MainPage`, `DisplayAlert`).

### Added

- GitHub Actions CI (build, test, packaging smoke test) and a tag-driven release workflow. It produces portable x64/arm64 zips, an optional signed MSIX and SHA-256 checksums.
- Dependabot for NuGet packages and GitHub Actions.
- `DriveLinker.Core.Tests` (xUnit v3). Tests cover encryption round-trips, compatibility with data written by earlier versions, and concurrency.
- Rewritten README: installation, building, data locations, security notes and the release process.

## [1.0] - 2024

- Initial releases.
