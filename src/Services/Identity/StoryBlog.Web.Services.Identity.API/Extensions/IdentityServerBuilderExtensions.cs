using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    /*internal static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddDeveloperSigningCredentials(
            this IIdentityServerBuilder builder,
            bool persistKey, 
            string filename = null)
        {
            if (null == filename)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "tempkey.rsa");
            }

            if (File.Exists(filename))
            {
                var keyFile = File.ReadAllText(filename);
                var tempKey = JsonSerializer.Parse<TemporaryRsaKey>(keyFile);

                return builder.AddSigningCredential(CreateRsaSecurityKey(tempKey.Parameters.ToRSAParameters(), tempKey.KeyId));
            }
            else
            {
                var key = CreateRsaSecurityKey();
                var parameters = key.Rsa?.ExportParameters(includePrivateParameters: true) ?? key.Parameters;

                var tempKey = new TemporaryRsaKey
                {
                    Parameters = RsaKeyParameters.Create(parameters),
                    KeyId = key.KeyId
                };

                if (persistKey)
                {
                    using (var writer = File.OpenWrite(filename))
                    {
                        var bytes = JsonSerializer.ToUtf8Bytes(tempKey);
                        writer.Write(bytes, 0, bytes.Length);
                    }

                    //File.WriteAllText(filename, JsonConvert.SerializeObject(tempKey, new JsonSerializerSettings { ContractResolver = new RsaKeyContractResolver() }));
                }

                return builder.AddSigningCredential(key);
            }

        }

        public static IIdentityServerBuilder AddSigninCredentials(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var keyId = configuration[nameof(RsaSecurityKey.KeyId)];

            return builder.AddSigningCredential(CreateRsaSecurityKey(configuration, keyId));
        }

        private static RsaSecurityKey CreateRsaSecurityKey(RSAParameters parameters, string id)
        {

            var key = new RsaSecurityKey(parameters)
            {
                KeyId = id
            };

            return key;
        }

        private static RsaSecurityKey CreateRsaSecurityKey(IConfiguration configuration, string id)
        {
            var section = configuration.GetSection(nameof(RsaSecurityKey.Parameters));
            var keyId = configuration[nameof(RsaSecurityKey.KeyId)];
            var rsaParameters = new RSAParameters
            {
                D = Convert.FromBase64String(section[nameof(RSAParameters.D)]),
                DP = Convert.FromBase64String(section[nameof(RSAParameters.DP)]),
                DQ = Convert.FromBase64String(section[nameof(RSAParameters.DQ)]),
                Exponent = Convert.FromBase64String(section[nameof(RSAParameters.Exponent)]),
                InverseQ = Convert.FromBase64String(section[nameof(RSAParameters.InverseQ)]),
                Modulus = Convert.FromBase64String(section[nameof(RSAParameters.Modulus)]),
                P = Convert.FromBase64String(section[nameof(RSAParameters.P)]),
                Q = Convert.FromBase64String(section[nameof(RSAParameters.Q)])
            };

            return new RsaSecurityKey(rsaParameters)
            {
                KeyId = keyId
            };
        }

        private static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsa = RSA.Create();
            RsaSecurityKey key;

            if (rsa is RSACryptoServiceProvider)
            {
                rsa.Dispose();

                using (var cng = new RSACng(2048))
                {
                    var parameters = cng.ExportParameters(includePrivateParameters: true);
                    key = new RsaSecurityKey(parameters);
                }
            }
            else
            {
                rsa.KeySize = 2048;
                key = new RsaSecurityKey(rsa);
            }

            key.KeyId = CryptoRandom.CreateUniqueId(16);
            return key;
        }

        [Serializable]
        internal class RsaKeyParameters
        {
            [JsonPropertyName(nameof(D))]
            public byte[] D
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(DP))]
            public byte[] DP
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(DQ))]
            public byte[] DQ
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(Exponent))]
            public byte[] Exponent
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(InverseQ))]
            public byte[] InverseQ
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(Modulus))]
            public byte[] Modulus
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(P))]
            public byte[] P
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(Q))]
            public byte[] Q
            {
                get;
                set;
            }

            public RSAParameters ToRSAParameters()
            {
                return new RSAParameters
                {
                    D = D,
                    DP = DP,
                    DQ = DQ,
                    Exponent = Exponent,
                    InverseQ = InverseQ,
                    Modulus = Modulus,
                    P = P,
                    Q = Q
                };
            }

            public static RsaKeyParameters Create(RSAParameters parameters)
            {
                return new RsaKeyParameters
                {
                    D = parameters.D,
                    DP = parameters.DP,
                    DQ = parameters.DQ,
                    Exponent = parameters.Exponent,
                    InverseQ = parameters.InverseQ,
                    Modulus = parameters.Modulus,
                    P = parameters.P,
                    Q = parameters.Q
                };
            }
        }

        [Serializable]
        internal class TemporaryRsaKey
        {
            [JsonPropertyName(nameof(KeyId))]
            public string KeyId
            {
                get;
                set;
            }

            [JsonPropertyName(nameof(Parameters))]
            public RsaKeyParameters Parameters
            {
                get;
                set;
            }
        }
    }*/
}