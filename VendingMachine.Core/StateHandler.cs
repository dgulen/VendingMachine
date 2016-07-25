using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VendingMachine.Core
{
    public class StateHandler
    {
        State _state = State.UNKNOWN;
        Semaphore _sema = new Semaphore(1, 1);

        public void UpdateState(State newState)
        {
            _sema.WaitOne();
            _state = newState;
            _sema.Release();
        }

        public State GetState()
        {
            State retVal = State.UNKNOWN;

            _sema.WaitOne();
            retVal = _state;
            _sema.Release();

            return retVal;
        }

        public static bool CheckStateHandler(ref StateHandler _localStateHandler)
        {
            if (_localStateHandler.GetState() != State.ALLOW_RUNNING)
            {
                while (true)
                {
                    if (_localStateHandler.GetState() == State.ASKING_USER)
                    {
                        Thread.Sleep(100);
                    }
                    else if (_localStateHandler.GetState() == State.ALLOW_RUNNING)
                    {
                        return true;
                    }
                    else if (_localStateHandler.GetState() == State.USER_CANCELLED)
                    {
                        return false;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            return true;
        }
    }
}
