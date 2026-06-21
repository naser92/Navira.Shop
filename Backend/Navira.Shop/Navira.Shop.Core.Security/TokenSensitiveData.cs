using Newtonsoft.Json;

namespace Navira.Shop.Core.Security
{
    public class TokenSensitiveData
    {
        public Guid UserId { get; set; }
        public Guid? TraceId { get; set; }
        public bool IsAdmin { get; set; }
        public int? UserType { get; set; }

        public static TokenSensitiveData Decrypt(string content)
        {
            var decrypted = content.Decrypted();
            var data = JsonConvert.DeserializeObject<TokenSensitiveData>(decrypted);
            return data;
        }

        public string Encrypt()
        {
            var jsonData = JsonConvert.SerializeObject(this);
            var encrypted = jsonData.Encrypted();

            return encrypted;
        }
    }


}
