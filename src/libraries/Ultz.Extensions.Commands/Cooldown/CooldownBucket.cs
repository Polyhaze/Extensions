using System;
using System.Threading;

namespace Ultz.Extensions.Commands.Cooldown
{
    internal sealed class CooldownBucket
    {
        private int _remaining;

        public CooldownBucket(Cooldown cooldown)
        {
            Cooldown = cooldown;
            _remaining = Cooldown.Amount;
        }

        public Cooldown Cooldown { get; }

        public int Remaining => Volatile.Read(ref _remaining);

        public DateTimeOffset Window { get; private set; }

        public DateTimeOffset LastCall { get; private set; }

        public bool IsRateLimited(out TimeSpan retryAfter)
        {
            var now = DateTimeOffset.UtcNow;
            LastCall = now;

            if (Remaining == Cooldown.Amount)
            {
                Window = now;
            }

            if (now > Window + Cooldown.Per)
            {
                _remaining = Cooldown.Amount;
                Window = now;
            }

            if (Remaining == 0)
            {
                retryAfter = Cooldown.Per - (now - Window);
                return true;
            }

            retryAfter = default;
            return false;
        }

        public void Decrement()
        {
            var now = DateTimeOffset.UtcNow;
            Interlocked.Decrement(ref _remaining);

            if (Remaining == 0)
            {
                Window = now;
            }
        }

        public void Reset()
        {
            _remaining = Cooldown.Amount;
            LastCall = default;
            Window = default;
        }
    }
}