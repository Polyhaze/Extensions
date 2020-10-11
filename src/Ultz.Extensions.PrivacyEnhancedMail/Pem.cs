// 
// Pem.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#endregion

namespace Ultz.Extensions.PrivacyEnhancedMail
{
    /// <summary>
    ///     A class containing PEM format helper methods
    /// </summary>
    public static class Pem
    {
        /// <summary>
        ///     Gets a <see cref="X509Certificate2" /> from a Base64-encoded certificate.
        /// </summary>
        /// <param name="cert"></param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificate(string cert)
        {
            return new X509Certificate2(PemExt.GetBytesFromPem(cert, "CERTIFICATE"));
        }

#if NET472 || NETCOREAPP2_0 || NETSTANDARD2_1
        /// <summary>
        ///     Gets a <see cref="X509Certificate2" /> from a Base64-encoded certificate and private key.
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificate(string cert, string key)
        {
            return GetCertificate(cert)
                .CopyWithPrivateKey
                (
                    PemExt.DecodeRsaPrivateKey
                    (
                        PemExt.GetBytesFromPem
                        (
                            key,
                            "RSA PRIVATE KEY"
                        )
                    )
                );
        }
#endif

        public static (X509Certificate2, RSA) GetCertificateAndKey(string cert, string key)
        {
            return (GetCertificate(cert), PemExt.DecodeRsaPrivateKey(PemExt.GetBytesFromPem(key, "RSA PRIVATE KEY")));
        }
    }
}
