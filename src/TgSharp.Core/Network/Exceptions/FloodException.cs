using System;

namespace TgSharp.Core.Network.Exceptions
{
    public class FloodException : Exception
    {
        public TimeSpan TimeToWait { get; private set; }

        internal FloodException(TimeSpan timeToWait)
            : base($"Telegram now requires your program to do requests again only after {timeToWait.TotalSeconds} seconds have passed.")
        {
            TimeToWait = timeToWait;
        }
    }
}