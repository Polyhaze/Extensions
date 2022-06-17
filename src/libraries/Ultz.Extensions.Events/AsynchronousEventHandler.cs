using System;
using System.Threading.Tasks;

namespace Ultz.Extensions.Events
{
    /// <summary>
    /// Represents an asynchronous event handler used by the <see cref="AsynchronousEvent{T}" />.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type" /> of <see cref="EventArgs" /> used by this handler. </typeparam>
    /// <param name="e"> The <see cref="EventArgs" /> object containing the event data. </param>
    public delegate Task AsynchronousEventHandler<in T>(T e) where T : EventArgs;
}