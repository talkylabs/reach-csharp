using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;

namespace Reach.Converters
{
    /// <summary>
    /// PaymentInfo from Reach API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentInfo
    {
        /// <summary>
        /// The payee
        /// </summary>
        [JsonProperty("payee")]
        public string Payee { get; set; }

        /// <summary>
        /// The amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set;}


        /// <summary>
        /// Create an empty PaymentInfo
        /// </summary>
        public PaymentInfo() {}
        private PaymentInfo(
            [JsonProperty("payee")]
            string payee,
            [JsonProperty("amount")]
            decimal amount,
            [JsonProperty("currency")]
            string currency
        ) {
            Payee = payee;
            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Create a PaymentInfo from a JSON payload
        /// </summary>
        /// <param name="json">JSON string to parse</param>
        public static PaymentInfo FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PaymentInfo>(json);
        }

        public override string ToString() {
    	    StringBuilder sb = new StringBuilder();
    	    sb.Append("class PaymentInfo {\n");

    	    sb.Append("    payee: ").Append(toIndentedString(Payee)).Append("\n");
    	    sb.Append("    amount: ").Append(toIndentedString(Amount)).Append("\n");
    	    sb.Append("    currency: ").Append(toIndentedString(Currency)).Append("\n");
    	    sb.Append("}");
    	    return sb.ToString();
    	  }

        public string AsRequestParam() {
  		    StringBuilder sb = new StringBuilder();
  		    sb.Append("{");
  		    sb.Append("\"payee\": \"").Append(Payee==null?"null":Payee);
  		    sb.Append("\", \"amount\": \"").Append(Amount.ToString());
  		    sb.Append("\", \"currency\": \"").Append(Currency==null?"null":Currency);
  		    sb.Append("\"}");
  		    return sb.ToString();
  		  }

        private string toIndentedString(object o) {
    	    if (o == null) {
    	      return "null";
    	    }
    	    return o.ToString().Replace("\n", "\n    ");
    	  }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(GetType()))
            {
                return false;
            }

            var o = (PaymentInfo) Convert.ChangeType(obj, GetType());
            if (o == null)
            {
                return false;
            }

            return o.Payee == Payee && o.Amount == Amount && o.Currency == Currency;
        }

        public override int GetHashCode()
        {
            int result = Payee==null?0: Payee.GetHashCode();
            result = result + (Amount.GetHashCode());
            result = result + (Currency==null?0: Currency.GetHashCode());
            return result;
        }

        public static bool operator ==(PaymentInfo a, PaymentInfo b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator!=(PaymentInfo a, PaymentInfo b)
        {
            return !(a == b);
        }
    }
}
