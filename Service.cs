using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace SillyService
{
    public partial class Service : ServiceBase
    {
        /// <summary>
        /// Main times
        /// </summary>
        private System.Timers.Timer timer2nextUpdate;
        /// <summary>
        /// Timer interval in seconds
        /// </summary>
        private int timerInterval = Properties.Settings.Default.timerInterval;
        // here you can see how this value is being pulled out from Settings

        public Service()
        {
            InitializeComponent();
            
            // create new Source if it doesn't exist
            EventSourceCreationData escd = new EventSourceCreationData(eventLog.Source, eventLog.Log);
            if (!EventLog.SourceExists(eventLog.Source))
            {
                EventLog.CreateEventSource(escd);
            }
        }

        protected override void OnStart(string[] args)
        {
            // using System.Text;
            StringBuilder greet = new StringBuilder()
                .Append("SillyService has been started.\n\n")
                .Append(string.Format("Timer interval (in seconds): {0}\n", timerInterval))
                // using System.Reflection;
                .Append(string.Format("Path to the executable: {0}", Assembly.GetExecutingAssembly().Location));
            write2log(greet.ToString(), EventLogEntryType.Information);

            // timer settings
            this.timer2nextUpdate = new System.Timers.Timer(timerInterval * 1000);
            this.timer2nextUpdate.AutoReset = true;
            this.timer2nextUpdate.Elapsed
                // what timer's event will do
                += new System.Timers.ElapsedEventHandler(this.timer2nextUpdate_tick);
            this.timer2nextUpdate.Start();
        }

        protected override void OnStop()
        {
            write2log("SillyService has been stopped", EventLogEntryType.Information);
        }

        /// <summary>
        /// Writing to log
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="type">type of the event</param>
        private void write2log(string message, EventLogEntryType type)
        {
            try { eventLog.WriteEntry(message, type); } catch { }
        }

        /// <summary>
        /// timer's event
        /// </summary>
        private void timer2nextUpdate_tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            write2log("ololo", EventLogEntryType.Information);
        }
    }
}
