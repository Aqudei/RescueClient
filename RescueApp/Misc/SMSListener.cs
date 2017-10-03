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
using RescueApp.Messages;
using Newtonsoft.Json;

namespace RescueApp.Misc
{
    public class SMSListener
    {
        private GsmCommMain gsmComm;
        private BackgroundWorker worker;

        public SMSListener(string smsPort, int smsBaud)
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            LoadReloadListener(smsPort, smsBaud);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                Messenger.Default.Send<NewCheckInMessage>(new NewCheckInMessage
                {
                    CheckInInfo = JsonConvert.DeserializeObject<CheckInInfo>(e.UserState.ToString())
                });
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (gsmComm.IsOpen())
            {
                var msgs = gsmComm.ReadMessages(PhoneMessageStatus.ReceivedUnread, "SM");
                foreach (var msg in msgs)
                {
                    var sms = (SmsDeliverPdu)msg.Data;
                    worker.ReportProgress(0, sms.UserDataText);
                    gsmComm.DeleteMessage(msg.Index, msg.Storage);
                }
            }
        }

        public void LoadReloadListener(string portnumber, int baudrate)
        {


            if (gsmComm == null)
            {
                gsmComm = new GsmCommMain(portnumber, baudrate);
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
                if (gsmComm.IsOpen())
                    gsmComm.Close();
                gsmComm = new GsmCommMain(portnumber, baudrate);
                gsmComm.Open();
            }

            worker.RunWorkerAsync();
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
                    .DeserializeObject<CheckInInfo>(data.UserDataText);

                Messenger.Default.Send(new Messages.NewCheckInMessage
                {
                    CheckInInfo = checkNInfo
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
