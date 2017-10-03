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
using System.Threading;

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

        public event EventHandler<NewCheckInMessage> NewMessageReceived;

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                try
                {
                    var chkin_info = JsonConvert.DeserializeObject<CheckInInfo>(e.UserState.ToString());
                    NewMessageReceived?.Invoke(this, new NewCheckInMessage
                    {
                        CheckInInfo = chkin_info
                    });
                }
                catch (Exception)
                {
                    Debug.WriteLine("SMS NOT IN VALID FORMAT");
                }
                finally
                {
                    Debug.WriteLine(e.UserState.ToString());
                }

            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (gsmComm.IsConnected())
            {
                var msgs = gsmComm.ReadMessages(PhoneMessageStatus.ReceivedUnread, "SM");
                Thread.Sleep(512);
                foreach (var msg in msgs)
                {
                    var sms = (SmsDeliverPdu)msg.Data;
                    worker.ReportProgress(0, sms.UserDataText);
                    gsmComm.DeleteMessage(msg.Index, msg.Storage);
                    Thread.Sleep(512);
                }
            }

            Debug.WriteLine("Worker Thread Terminated");
        }

        public void LoadReloadListener(string portnumber, int baudrate)
        {
            if (gsmComm == null)
            {
                gsmComm = new GsmCommMain(portnumber, baudrate, 1000);
                try
                {
                    gsmComm.Open();
                    worker.RunWorkerAsync();
                    Debug.WriteLine("RUNNING WITH SMS SUPPORT");
                }
                catch (Exception)
                {
                    Debug.WriteLine("RUNNING WITH NO SMS SUPPORT");
                }
            }
            else
            {
                try
                {
                    if (gsmComm.IsConnected())
                        gsmComm.Close();
                    gsmComm = new GsmCommMain(portnumber, baudrate, 1000);
                    gsmComm.Open();
                    worker.RunWorkerAsync();
                    Debug.WriteLine("RUNNING WITH SMS SUPPORT");
                }
                catch (Exception)
                {
                    Debug.WriteLine("RUNNING WITH NO SMS SUPPORT");
                }
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

        public void Terminate()
        {
            try
            {
                if (gsmComm != null)
                    if (gsmComm.IsConnected())
                        gsmComm.Close();
            }
            catch (Exception)
            { }
        }
    }
}
