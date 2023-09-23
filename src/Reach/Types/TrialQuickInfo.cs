using System.Collections.Generic;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Reach.Types
{
    /// <summary>
    /// PaymentInfo from Reach API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TrialQuickInfo
    {
        /// <summary>
        /// The creation date
        /// </summary>
        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The Id of the Trial
        /// </summary>
        [JsonProperty("trialId")]
        public string TrialId { get; set; }

        /// <summary>
        /// The channel of the Trial
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set;}


        /// <summary>
        /// Create an empty PaymentInfo
        /// </summary>
        public TrialQuickInfo() {}
        private TrialQuickInfo(
            [JsonProperty("dateCreated")]
            DateTime dateCreated,
            [JsonProperty("trialId")]
            string trialId,
            [JsonProperty("channel")]
            string channel
        ) {
            DateCreated = dateCreated;
            TrialId = trialId;
            Channel = channel;
        }

        /// <summary>
        /// Create a PaymentInfo from a JSON payload
        /// </summary>
        /// <param name="json">JSON string to parse</param>
        public static TrialQuickInfo FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TrialQuickInfo>(json);
        }

        public override string ToString() {
    	    StringBuilder sb = new StringBuilder();
    	    sb.Append("class TrialQuickInfo {\n");

    	    sb.Append("    dateCreated: ").Append(DateCreated==null?"null":toIndentedString(DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ"))).Append("\n");
    	    sb.Append("    trialId: ").Append(toIndentedString(TrialId)).Append("\n");
    	    sb.Append("    channel: ").Append(toIndentedString(Channel)).Append("\n");
    	    sb.Append("}");
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

            var o = (TrialQuickInfo) Convert.ChangeType(obj, GetType());
            if (o == null)
            {
                return false;
            }

            return o.DateCreated == DateCreated && o.TrialId == TrialId && o.Channel == Channel;
        }

        public override int GetHashCode()
        {
            int result = DateCreated==null?0: DateCreated.GetHashCode();
            result = result + (TrialId==null?0: TrialId.GetHashCode());
            result = result + (Channel==null?0: Channel.GetHashCode());
            return result;
        }

        public static bool operator ==(TrialQuickInfo a, TrialQuickInfo b)
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

        public static bool operator!=(TrialQuickInfo a, TrialQuickInfo b)
        {
            return !(a == b);
        }
    }
}
