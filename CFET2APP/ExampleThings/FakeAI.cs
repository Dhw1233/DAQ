using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Attributes;
using Jtext103.CFET2.Core.Event;
using Jtext103.CFET2.Core.Extension;
using Jtext103.CFET2.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.ExampleThings
{
    public class FakeAIThing : Thing
    {
        private FakeAICard myFakeAICard;

        public override void TryInit(object channelCount = null)
        {
            //myFakeAICard = new FakeAICard((int)channelCount);
            myFakeAICard = new FakeAICard(2);
        }

        [Cfet2Config(ConfigActions = ConfigAction.Set, Name = "ChannelCountConfig")]
        public void ChannelCountConfigSet(int channelCount)
        {
            myFakeAICard.ChannelCount = channelCount;
        }

        [Cfet2Config(ConfigActions = ConfigAction.Get, Name = "ChannelCountConfig")]
        public int ChannelCountConfigGet()
        {
            return myFakeAICard.ChannelCount;
        }

        [Cfet2Status]
        public int ChannelCount()
        {
            return myFakeAICard.ChannelCount;
        }

        [Cfet2Status]
        public Status AIState()
        {
            return myFakeAICard.AIState;
        }

        [Cfet2Status]
        public double[] LatestData()
        {
            return myFakeAICard.LatestData;
        }

        [Cfet2Method]
        public void TryArm()
        {
            myFakeAICard.TryArm();
        }

        
        [Cfet2Method]
        public void TryStop()
        {
            myFakeAICard.TryStop();
        }        
    }

    public class FakeAICard
    {
        public int ChannelCount { get; set; }

        public double[] LatestData { get; private set; }

        public Status AIState { get; private set; }

        private Thread myAcuqisition;

        public FakeAICard(int channelCount)
        {
            ChannelCount = channelCount;
        }

        public void TryArm()
        {
            if(AIState == Status.Idle)
            {
                LatestData = new double[ChannelCount];
                myAcuqisition = new Thread(FakeAcquisition);
                myAcuqisition.Start();
                AIState = Status.Running;
            }        
        }

        private void FakeAcquisition()
        {
            Random rd = new Random();
            while(true)
            {
                for(int i = 0; i < LatestData.Length; i++)
                {
                    LatestData[i] = rd.NextDouble() + i;
                }
                Thread.Sleep(100);
            }
        }

        public void TryStop()
        {
            if(AIState == Status.Running)
            {
                myAcuqisition.Abort();
                myAcuqisition.DisableComObjectEagerCleanup();
                AIState = Status.Idle;
            }
        }
    }

    public enum Status
    {
        Idle = 0,
        Running = 2
    }
}
