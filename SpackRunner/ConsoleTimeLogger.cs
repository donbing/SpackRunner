using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SpackRunner
{
	public static class ConsoleTimeLogger
	{
		public class Logger : IDisposable
		{
			readonly DateTime d;
			readonly string name;
			public List<TimeSpan> timers = new List<TimeSpan> ();

			public TimeSpan Average{ get { return timers.Any () ? TimeSpan.FromTicks (Convert.ToInt64 (timers.Average (x => x.Ticks))) : TimeSpan.Zero; } }

			public Logger (string name)
			{
				this.name = name;
				d = DateTime.Now;
			}

			public Averager Measure ()
			{
				return new Averager (this);
			}

			public class Averager : IDisposable
			{
				Logger log;
				DateTime start;

				public Averager (Logger log)
				{
					this.log = log;	
					this.start = DateTime.Now;
				}

				public void Dispose ()
				{
					log.timers.Add (DateTime.Now - start);
				}
			}

			public TimeSpan Timed { get { return DateTime.Now - d; } }

			public void Dispose ()
			{
				//Console.Out.WriteLine (string.Format ("{0}:{1} ave:{2} tot:{3}", name, DateTime.Now - d, Average, timers.Count));
			}
		}

		public static Logger Log (string name)
		{
			return new Logger (name);
		}

		public static Logger Log (string format, params object[] args)
		{
			return new Logger (string.Format (format, args));
		}
	}
}