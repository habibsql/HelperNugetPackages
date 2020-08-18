using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Xunit;

namespace Utitlity.Nuget.Packages.SimpleShortMessage
{
    /* Nuget package Dependency: Twilio 
     * Also OAuth registration is needed. Twilio support OAuth protocol
       More detail: https://www.twilio.com/messaging
    */

    /// <summary>
    /// Send whats app message from your application
    /// </summary>
    public class WhatsAppMessageSendingTests
    {
        private const string TwilioWhatsAppAccountId = "abcd354effa688d2f53a517f29ff920aa"; //fake. should be valid
        private const string TwilioWhatsAppSecretKey = "abcd28e6700769dbe85df3a8296aa"; //fake. should be valid

        [Fact]
        public async Task SendWhatsAppMessage()
        {
            const string phoneNumberFrom = "+11 1234 99999"; //need to purchased from Twilio
            const string phoneNumberTo = "+01819112030"; //valid phone numer
            const string message = "Hi! Eid Mubarak!";

            TwilioClient.Init(TwilioWhatsAppAccountId, TwilioWhatsAppSecretKey);

            // whatsapp phonenumber pattern: new PhoneNumber("whatsapp:+123456789") 
            // "From phonenumber" must be purchased from twilio (it is not free)
            MessageResource messageResource = await MessageResource.CreateAsync(
                from: new PhoneNumber(phoneNumberFrom),
                to: new PhoneNumber(phoneNumberTo),
                body: message);

            Assert.Empty(messageResource.ErrorMessage);
        }
    }
}
