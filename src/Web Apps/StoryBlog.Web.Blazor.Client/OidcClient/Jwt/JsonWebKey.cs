using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Jwt
{
    /// <summary>
    /// Represents a Json Web Key as defined in http://tools.ietf.org/html/rfc7517.
    /// </summary>
    //[JsonObject]
    public class JsonWebKey
    {
        // kept private to hide that a List is used.
        // public member returns an IList.
        private IList<string> _certificateClauses = new List<string>();
        private IList<string> keyops = new List<string>();

        /// <summary>
        /// Gets or sets the 'alg' (KeyType)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Alg, Required = Required.Default)]
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the 'crv' (ECC - Curve)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Crv, Required = Required.Default)]
        public string Crv { get; set; }

        /// <summary>
        /// Gets or sets the 'd' (ECC - Private Key OR RSA - Private Exponent)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.D, Required = Required.Default)]
        public string D { get; set; }

        /// <summary>
        /// Gets or sets the 'dp' (RSA - First Factor CRT Exponent)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.DP, Required = Required.Default)]
        public string DP { get; set; }

        /// <summary>
        /// Gets or sets the 'dq' (RSA - Second Factor CRT Exponent)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.DQ, Required = Required.Default)]
        public string DQ { get; set; }

        /// <summary>
        /// Gets or sets the 'e' (RSA - Exponent)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.E, Required = Required.Default)]
        public string E { get; set; }

        /// <summary>
        /// Gets or sets the 'k' (Symmetric - Key Value)..
        /// </summary>
        /// Base64urlEncoding
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.K, Required = Required.Default)]
        public string K { get; set; }

        /// <summary>
        /// Gets or sets the 'key_ops' (Key Operations)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.KeyOps, Required = Required.Default)]
        public IList<string> KeyOps
        {
            get
            {
                return keyops;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("KeyOps");

                foreach (string keyOp in value)
                    keyops.Add(keyOp);
            }
        }

        /// <summary>
        /// Gets or sets the 'kid' (Key ID)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Kid, Required = Required.Default)]
        public string Kid { get; set; }

        /// <summary>
        /// Gets or sets the 'kty' (Key Type)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Kty, Required = Required.Default)]
        public string Kty { get; set; }

        /// <summary>
        /// Gets or sets the 'n' (RSA - Modulus)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlEncoding</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.N, Required = Required.Default)]
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the 'oth' (RSA - Other Primes Info)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Oth, Required = Required.Default)]
        public IList<string> Oth { get; set; }

        /// <summary>
        /// Gets or sets the 'p' (RSA - First Prime Factor)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.P, Required = Required.Default)]
        public string P { get; set; }

        /// <summary>
        /// Gets or sets the 'q' (RSA - Second  Prime Factor)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Q, Required = Required.Default)]
        public string Q { get; set; }

        /// <summary>
        /// Gets or sets the 'qi' (RSA - First CRT Coefficient)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlUInt</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.QI, Required = Required.Default)]
        public string QI { get; set; }

        /// <summary>
        /// Gets or sets the 'use' (Public Key Use)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Use, Required = Required.Default)]
        public string Use { get; set; }

        /// <summary>
        /// Gets or sets the 'x' (ECC - X Coordinate)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlEncoding</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X, Required = Required.Default)]
        public string X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the 'x5c' collection (X.509 Certificate Chain)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5c, Required = Required.Default)]
        public IList<string> X5c
        {
            get => _certificateClauses;
            set
            {
                //if (value == null)
                //    throw LogHelper.LogException<ArgumentNullException>(LogMessages.IDX10001, "X5c");

                foreach (var clause in value)
                {
                    _certificateClauses.Add(clause);
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'x5t' (X.509 Certificate SHA-1 thumbprint)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5t, Required = Required.Default)]
        public string X5t { get; set; }

        /// <summary>
        /// Gets or sets the 'x5t#S256' (X.509 Certificate SHA-1 thumbprint)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5tS256, Required = Required.Default)]
        public string X5tS256
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the 'x5u' (X.509 URL)..
        /// </summary>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5u, Required = Required.Default)]
        public string X5u
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the 'y' (ECC - Y Coordinate)..
        /// </summary>
        /// <remarks> value is formated as: Base64urlEncoding</remarks>
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Y, Required = Required.Default)]
        public string Y
        {
            get;
            set;
        }

        public int KeySize
        {
            get
            {
                switch (Kty)
                {
                    case JsonWebAlgorithmsKeyTypes.RSA:
                    {
                        return Base64Url.Decode(N).Length * 8;
                    }

                    case JsonWebAlgorithmsKeyTypes.EllipticCurve:
                    {
                        return Base64Url.Decode(X).Length * 8;
                    }

                    case JsonWebAlgorithmsKeyTypes.Octet:
                    {
                        return Base64Url.Decode(K).Length * 8;
                    }

                    default:
                    {
                        return 0;
                    }
                }
            }
        }

        public bool HasPrivateKey
        {
            get
            {
                if (Kty == JsonWebAlgorithmsKeyTypes.RSA)
                {
                    return null != D && null != DP && null != DQ && null != P && null != Q && null != QI;
                }

                if (Kty == JsonWebAlgorithmsKeyTypes.EllipticCurve)
                {
                    return null != D;
                }

                return false;
            }
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey"/>.
        /// </summary>
        public JsonWebKey()
        {
            _certificateClauses = new List<string>();
            keyops = new List<string>();
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey"/> from a json string.
        /// </summary>
        /// <param name="json">a string that contains JSON Web Key parameters in JSON format.</param>
        public JsonWebKey(string json)
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json");
            }

            var key = Microsoft.JSInterop.Json.Deserialize<JsonWebKey>(json);
            //var key = JsonConvert.DeserializeObject<JsonWebKey>(json);
            Copy(key);
        }

        private void Copy(JsonWebKey key)
        {
            Alg = key.Alg;
            Crv = key.Crv;
            D = key.D;
            DP = key.DP;
            DQ = key.DQ;
            E = key.E;
            K = key.K;

            if (null != key.KeyOps)
            {
                keyops = new List<string>(key.KeyOps);
            }

            Kid = key.Kid;
            Kty = key.Kty;
            N = key.N;
            Oth = key.Oth;
            P = key.P;
            Q = key.Q;
            QI = key.QI;
            Use = key.Use;

            if (key.X5c != null)
            {
                _certificateClauses = new List<string>(key.X5c);
            }

            X5t = key.X5t;
            X5tS256 = key.X5tS256;
            X5u = key.X5u;
            X = key.X;
            Y = key.Y;
        }
    }
}