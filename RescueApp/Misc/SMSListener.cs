using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsmComm.GsmCommunication;
using System.Diagnostics;
using GsmComm.PduConverter;
using RescueApp.Models;
using GalaSoft.MvvmLight.Messaging;

namespace RescueApp.Misc
{
    public class SMSListener
    {
        private GsmCommMain gsmComm;

        public SMSListener(int smsPort, int smsBaud)
        {
            LoadReloadListener(smsPort, smsBaud);
        }

        public void LoadReloadListener(int portnumber, int baudrate)
        {
            if (gsmComm == null)
            {
                gsmComm = new GsmCommMain(portnumber, baudrate);
                gsmComm.MessageReceived += GsmComm_MessageReceived;
                gsmComm.EnableMessageNotifications();                
                try
                {
                    gsmComm.Open();
                }
                catch (Exception)
                {
                    Debug.WriteLine("RUNNING WITH NO SMS SUPPORT");
                }
            }
            else
            {
                gsmComm.MessageReceived -= GsmComm_MessageReceived;
                if (gsmComm.IsOpen())
                    gsmComm.Close();
                gsmComm = new GsmCommMain(portnumber, baudrate);
                gsmComm.MessageReceived += GsmComm_MessageReceived;
            }
        }

        private void GsmComm_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                IMessageIndicationObject obj = e.IndicationObject;
                if (obj is MemoryLocation)
                {
                    MemoryLocation loc = (MemoryLocation)obj;
                    Debug.WriteLine(string.Format("New message received in storage \"{0}\", index {1}.", loc.Storage, loc.Index));
                    Debug.Write("");
                    return;
                }
                if (obj is ShortMessage)
                {
                    ShortMessage msg = (ShortMessage)obj;
                    SmsPdu pdu = gsmComm.DecodeReceivedMessage(msg);
                    Debug.WriteLine("New message received:");
                    ExtractSMSText(pdu);
                    return;
                }
                Debug.WriteLine("Error: Unknown notification object!");
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        private void ExtractSMSText(SmsPdu pdu)
        {
            if (pdu is SmsSubmitPdu)
            {
                // Stored (sent/unsent) message
                SmsSubmitPdu data = (SmsSubmitPdu)pdu;
                Debug.WriteLine("SENT/UNSENT MESSAGE");
                Debug.WriteLine("Recipient: " + data.DestinationAddress);
                Debug.WriteLine("Message text: " + data.UserDataText);
                Debug.WriteLine("-------------------------------------------------------------------");
                return;
            }
            if (pdu is SmsDeliverPdu)
            {
                // Received message
                SmsDeliverPdu data = (SmsDeliverPdu)pdu;
                Debug.WriteLine("RECEIVED MESSAGE");
                Debug.WriteLine("Sender: " + data.OriginatingAddress);
                Debug.WriteLine("Sent: " + data.SCTimestamp.ToString());
                Debug.WriteLine("Message text: " + data.UserDataText);

                var checkNInfo = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<ChecInInfo>(data.UserDataText);

                Messenger.Default.Send(new Messages.NewCheckInMessage
                {
                    ChecInInfo = checkNInfo
                });

                Debug.WriteLine("-------------------------------------------------------------------");
                return;
            }
            if (pdu is SmsStatusReportPdu)
            {
                // Status report
                SmsStatusReportPdu data = (SmsStatusReportPdu)pdu;
                Debug.WriteLine("STATUS REPORT");
                Debug.WriteLine("Recipient: " + data.RecipientAddress);
                Debug.WriteLine("Status: " + data.Status.ToString());
                Debug.WriteLine("Timestamp: " + data.DischargeTime.ToString());
                Debug.WriteLine("Message ref: " + data.MessageReference.ToString());
                Debug.WriteLine("-------------------------------------------------------------------");
                return;
            }
            Debug.WriteLine("Unknown message type: " + pdu.GetType().ToString());
        }
    }
}
