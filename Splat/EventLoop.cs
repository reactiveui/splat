using System;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public static partial class EventLoop
    {
        public static IEventLoop Current 
        {
            get 
            {
                throw new Exception("Could not create EventLoop. This should never happen, your dependency resolver is broken");
            }
        }

        public static IEventLoop MainLoop
        {
            get 
            {
                throw new Exception("Could not create EventLoop. This should never happen, your dependency resolver is broken");
            }
        }

        public static Task<IEventLoop> Spawn()
        {
            throw new Exception("Could not create EventLoop. This should never happen, your dependency resolver is broken");
        }
    }
}

