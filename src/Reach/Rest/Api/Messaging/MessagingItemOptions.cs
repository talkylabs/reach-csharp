/*
 * This code was generated by
 *  ___ ___   _   ___ _  _    _____ _   _    _  ___   ___      _   ___ ___      ___   _   ___     ___ ___ _  _ ___ ___    _ _____ ___  ___ 
 * | _ \ __| /_\ / __| || |__|_   _/_\ | |  | |/ | \ / / |    /_\ | _ ) __|___ / _ \ /_\ |_ _|__ / __| __| \| | __| _ \  /_\_   _/ _ \| _ \
 * |   / _| / _ \ (__| __ |___|| |/ _ \| |__| ' < \ V /| |__ / _ \| _ \__ \___| (_) / _ \ | |___| (_ | _|| .` | _||   / / _ \| || (_) |   /
 * |_|_\___/_/ \_\___|_||_|    |_/_/ \_\____|_|\_\ |_| |____/_/ \_\___/___/    \___/_/ \_\___|   \___|___|_|\_|___|_|_\/_/ \_\_| \___/|_|_\
 *                                                                                                                                         
 * Reach Messaging API
 * Reach SMS API helps you add robust messaging capabilities to your applications.  Using this REST API, you can * send SMS messages * track the delivery of sent messages * schedule SMS messages to send at a later time * retrieve and modify message history
 *
 * NOTE: This class is auto generated by OpenAPI Generator.
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


using System;
using System.Collections.Generic;
using Reach.Base;
using Reach.Converters;




namespace Reach.Rest.Api.Messaging
{
    /// <summary> This operation allows to delete a message record from the applet account. Once the record is deleted, it will no longer appear in the API and the applet portal.  This operation needs the `messageId` of the message to be deleted. To delete multiple messages, this operation should be called as many times as needed since it can only delete one message at a time. Note: Attempting to delete an in-progress message record, i.e. a message whose status is not `delivered`, `failed`, `canceled`, `undelivered`, will result in an error.    </summary>
    public class DeleteMessagingItemOptions : IOptions<MessagingItemResource>
    {
        
        ///<summary> The identifier of the message to be updated. </summary> 
        public string MessageId { get; }



        /// <summary> Construct a new DeleteMessageOptions </summary>
        /// <param name="messageId"> The identifier of the message to be updated. </param>
        public DeleteMessagingItemOptions(string messageId)
        {
            MessageId = messageId;
        }

        
        /// <summary> Generate the necessary parameters </summary>
        public  List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (MessageId != null)
            {
                p.Add(new KeyValuePair<string, string>("messageId", MessageId));
            }
            return p;
        }
        

    }


    /// <summary> This operation allows to fetch the API record associated to a message.  This operation needs the `messageId` of the message to be fetched.    </summary>
    public class FetchMessagingItemOptions : IOptions<MessagingItemResource>
    {
    
        ///<summary> The identifier of the message to be updated. </summary> 
        public string MessageId { get; }



        /// <summary> Construct a new FetchMessageOptions </summary>
        /// <param name="messageId"> The identifier of the message to be updated. </param>
        public FetchMessagingItemOptions(string messageId)
        {
            MessageId = messageId;
        }

        
        /// <summary> Generate the necessary parameters </summary>
        public  List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (MessageId != null)
            {
                p.Add(new KeyValuePair<string, string>("messageId", MessageId));
            }
            return p;
        }
        

    }


    /// <summary> This operation allows to retrieve from the API message records that satisfied specified criteria.  When getting the message record list, results will be sorted on the `dateSent` field with the most recent message records appearing first.    </summary>
    public class ReadMessagingItemOptions : ReadOptions<MessagingItemResource>
    {
    
        ///<summary> Retrieve messages sent to only this phone number. The phone number in E.164 format of the message. </summary> 
        public string Dest { get; set; }

        ///<summary> Retrieve messages sent from only this phone number, in E.164 format, or alphanumeric sender ID. </summary> 
        public string Src { get; set; }

        ///<summary> Retrieve only messages that are assocaited with this `bulkIdentifier`. </summary> 
        public string BulkIdentifier { get; set; }

        ///<summary> Retrieve only messages sent at the specified date. Must be in ISO 8601 format. </summary> 
        public DateTime? SentAt { get; set; }

        ///<summary> Retrieve only messages sent after the specified datetime. Must be in ISO 8601 format. </summary> 
        public DateTime? SentAfter { get; set; }

        ///<summary> Retrieve only messages sent before the specified datetime. Must be in ISO 8601 format. </summary> 
        public DateTime? SentBefore { get; set; }




        
        /// <summary> Generate the necessary parameters </summary>
        public  override List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (Dest != null)
            {
                p.Add(new KeyValuePair<string, string>("dest", Dest));
            }
            if (Src != null)
            {
                p.Add(new KeyValuePair<string, string>("src", Src));
            }
            if (BulkIdentifier != null)
            {
                p.Add(new KeyValuePair<string, string>("bulkIdentifier", BulkIdentifier));
            }
            if (SentAt != null)
            {
                p.Add(new KeyValuePair<string, string>("sentAt", Serializers.DateTimeIso8601(SentAt)));
            }
            if (SentAfter != null)
            {
                p.Add(new KeyValuePair<string, string>("sentAfter", Serializers.DateTimeIso8601(SentAfter)));
            }
            if (SentBefore != null)
            {
                p.Add(new KeyValuePair<string, string>("sentBefore", Serializers.DateTimeIso8601(SentBefore)));
            }
            if (PageSize != null)
            {
                p.Add(new KeyValuePair<string, string>("pageSize", PageSize.ToString()));
            }
            return p;
        }
        

    }


    /// <summary> This operation allows to send or schedule a message.  When sending a new message via the API, you must include the `dest` parameter.          This value should be the destination phone number. You must also include the `body` parameter containing the message's content as well as the `src` parameter   containing the sender alphanumeric Id or number.  To schedule a message, you must additionally pass the following parameter:  * `scheduledTime`: the date and time at which the sms will be sent in the ISO-8601 format.  </summary>
    public class SendMessagingItemOptions : IOptions<MessagingItemResource>
    {
        
        ///<summary> The destination phone number in E.164 format of the message. </summary> 
        public string Dest { get; }

        ///<summary> The phone number (in E.164 format), or the alphanumeric sender ID that initiated the message. </summary> 
        public string Src { get; }

        ///<summary> The text of the message to send. It can be up to 1,600 GSM-7 characters in length. That limit varies if your message is not made of only GSM-7 characters. More generally, the message body should not exceed 10 segments. </summary> 
        public string Body { get; }

        ///<summary> The identifier of the bulk operation this message belongs to. </summary> 
        public string BulkIdentifier { get; set; }

        ///<summary> The datetime at which the message will be sent. Must be in ISO 8601 format. A message must be scheduled at least 15 min in advance of message send time and cannot be scheduled more than 5 days in advance of the request. </summary> 
        public DateTime? ScheduledTime { get; set; }

        ///<summary> The URL that will be called to send status information of your message. If provided, the API POST these message status changes to the URL: `queued`, `failed`, `sent`, `canceled`, `delivered`, or `undelivered`. URLs must contain a valid hostname and underscores are not allowed.  </summary> 
        public string StatusCallback { get; set; }

        ///<summary> The maximum total price in the applet currency that should be paid for the message to be delivered. If the cost exceeds `maxPrice`, the message will fail and a status of `failed` is sent to the status callback.  </summary> 
        public decimal? MaxPrice { get; set; }

        ///<summary> It represents how long, in seconds, the message can remain in the queue. After this period elapses, the message fails and the status callback is called. After a message has been accepted by a carrier, however, there is no guarantee that the message will not be queued after this period. It is recommended that this value be at least 5 seconds. The maximum allowed value is 14,400 which corresponds to 4 hours.  </summary> 
        public int? ValidityPeriod { get; set; }


        /// <summary> Construct a new SendMessageOptions </summary>
        /// <param name="dest"> The destination phone number in E.164 format of the message. </param>
        /// <param name="src"> The phone number (in E.164 format), or the alphanumeric sender ID that initiated the message. </param>
        /// <param name="body"> The text of the message to send. It can be up to 1,600 GSM-7 characters in length. That limit varies if your message is not made of only GSM-7 characters. More generally, the message body should not exceed 10 segments. </param>
        public SendMessagingItemOptions(string dest, string src, string body)
        {
            Dest = dest;
            Src = src;
            Body = body;
        }

        
        /// <summary> Generate the necessary parameters </summary>
        public  List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (Dest != null)
            {
                p.Add(new KeyValuePair<string, string>("dest", Dest));
            }
            if (Src != null)
            {
                p.Add(new KeyValuePair<string, string>("src", Src));
            }
            if (Body != null)
            {
                p.Add(new KeyValuePair<string, string>("body", Body));
            }
            if (BulkIdentifier != null)
            {
                p.Add(new KeyValuePair<string, string>("bulkIdentifier", BulkIdentifier));
            }
            if (ScheduledTime != null)
            {
                p.Add(new KeyValuePair<string, string>("scheduledTime", Serializers.DateTimeIso8601(ScheduledTime)));
            }
            if (StatusCallback != null)
            {
                p.Add(new KeyValuePair<string, string>("statusCallback", StatusCallback.ToString()));
            }
            if (MaxPrice != null)
            {
                p.Add(new KeyValuePair<string, string>("maxPrice", MaxPrice.Value.ToString()));
            }
            if (ValidityPeriod != null)
            {
                p.Add(new KeyValuePair<string, string>("validityPeriod", ValidityPeriod.ToString()));
            }
            return p;
        }
        

    }
    /// <summary> This operation allows to cancel a previously scheduled message.  This operation needs the `messageId` of the message to be unscheduled. To unschedule multiple messages, this operation should be called as many times needed since it can only unschedule one message at a time.  Note: The system will make the best attempt to cancel a scheduled message when a request is received.  </summary>
    public class UnscheduleMessagingItemOptions : IOptions<MessagingItemResource>
    {
    
        ///<summary> The identifier of the message to be unscheduled. </summary> 
        public string MessageId { get; }



        /// <summary> Construct a new UnscheduleMessageOptions </summary>
        /// <param name="messageId"> The identifier of the message to be unscheduled. </param>
        public UnscheduleMessagingItemOptions(string messageId)
        {
            MessageId = messageId;
        }

        
        /// <summary> Generate the necessary parameters </summary>
        public  List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (MessageId != null)
            {
                p.Add(new KeyValuePair<string, string>("messageId", MessageId));
            }
            return p;
        }
        

    }


    /// <summary> This operation allows to update the body of a message. It is primarily used to redact the content of message while leaving all the other properties untouched.  This operation needs the `messageId` of the message to be updated. It also requires the `body` that will be newly associated with the message. To update multiple messages, this operation should be called as many times as needed since it can only update one message at a time.  Note: The previous body of the message is the one that is sent to the destination phone number. This operation just update the `body` in the API platform.  </summary>
    public class UpdateMessagingItemOptions : IOptions<MessagingItemResource>
    {
    
        ///<summary> The identifier of the message to be updated. </summary> 
        public string MessageId { get; }

        ///<summary> The text to be newly associated with the message. </summary> 
        public string Body { get; }



        /// <summary> Construct a new UpdateMessageOptions </summary>
        /// <param name="messageId"> The identifier of the message to be updated. </param>
        /// <param name="body"> The text to be newly associated with the message. </param>
        public UpdateMessagingItemOptions(string messageId, string body)
        {
            MessageId = messageId;
            Body = body;
        }

        
        /// <summary> Generate the necessary parameters </summary>
        public  List<KeyValuePair<string, string>> GetParams()
        {
            var p = new List<KeyValuePair<string, string>>();

            if (MessageId != null)
            {
                p.Add(new KeyValuePair<string, string>("messageId", MessageId));
            }
            if (Body != null)
            {
                p.Add(new KeyValuePair<string, string>("body", Body));
            }
            return p;
        }
        

    }


}
