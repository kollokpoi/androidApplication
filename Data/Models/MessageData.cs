using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomApi.Data.Models
{
    public class MessageData
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Message Message { get; set; }

        [NotMapped]
        private sbyte[] notMappedData;
        [NotMapped]
        public sbyte[] NotMappedData
        {
            get { return notMappedData; }
            set { 
                notMappedData = value;
                Data = new byte[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    Data[i] = (byte)(value[i] & 0xFF);
                }
            }
        }

        public byte[] Data { get; set; }
    }
}
