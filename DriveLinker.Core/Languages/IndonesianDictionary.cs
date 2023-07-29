﻿namespace DriveLinker.Core.Languages;
public class IndonesianDictionary : IIndonesianDictionary
{
    private const string CacheName = nameof(IndonesianDictionary);
    private readonly IMemoryCache _cache;

    public IndonesianDictionary(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<Keyword, string> GetIndonesianDictionary()
    {
        var output = _cache.Get<Dictionary<Keyword, string>>(CacheName);
        if (output is null)
        {
            output = GetDictionary();
            _cache.Set(CacheName, output);
        }

        return output;
    }

    private static Dictionary<Keyword, string> GetDictionary()
    {
        return new Dictionary<Keyword, string>()
        {
            { Keyword.Countdown, "Mundur Waktu" },
            { Keyword.CreateADrive, "Buat Drive" },
            { Keyword.Settings, "Pengaturan" },
            { Keyword.Search, "Cari" },
            { Keyword.RecentlyAdded, "Drive yang Baru Ditambahkan" },
            { Keyword.DriveListing, "Daftar Drive" },
            { Keyword.LinkDrives, "Hubungkan Drive" },
            { Keyword.UnlinkDrives, "Putuskan Hubungan Drive" },
            { Keyword.Letter, "Huruf" },
            { Keyword.LetterDesc, "Masukkan huruf drive Anda." },
            { Keyword.IpAddress, "Alamat IP" },
            { Keyword.IpAddressDesc, "Masukkan alamat IP drive Anda." },
            { Keyword.DriveName, "Nama Drive" },
            { Keyword.DriveNameDesc, "Masukkan nama drive Anda." },
            { Keyword.Password, "Kata Sandi" },
            { Keyword.PasswordDesc, "Masukkan kata sandi drive Anda." },
            { Keyword.UserName, "Nama Pengguna" },
            { Keyword.UserNameDesc, "Masukkan nama pengguna drive Anda." },
            { Keyword.DateCreated, "Tanggal Dibuat" },
            { Keyword.Create, "Buat" },
            { Keyword.AutoLinkDrives, "Hubungkan Drive secara Otomatis Saat Memulai" },
            { Keyword.AutoMinimize, "Minimalkan Aplikasi secara Otomatis" },
            { Keyword.ClearOnlyLetterDriveName, "Hapus Hanya Huruf / Nama Drive" },
            { Keyword.Save, "Simpan" },
            { Keyword.Language, "Bahasa" },
            { Keyword.English, "Inggris" },
            { Keyword.French, "Perancis" },
            { Keyword.German, "Jerman" },
            { Keyword.Indonesian, "Indonesia" },
            { Keyword.DontHaveAnAccount, "Belum punya akun?" },
            { Keyword.ForgotMyPassword, "Lupa kata sandi" },
            { Keyword.Login, "Masuk" },
            { Keyword.RecoveryKey, "Kunci Pemulihan" },
            { Keyword.RecoveryKeyDesc, "Masukkan salah satu kunci pemulihan Anda" },
            { Keyword.RecoveryKeyHelperText, "Anda dapat menggunakannya jika Anda lupa kata sandi Anda." },
            { Keyword.Copyclipboard, "Salin ke papan klip" },
            { Keyword.Register, "Daftar" },
        };
    }
}
