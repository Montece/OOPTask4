using Xunit;

namespace OOPTask4.Threading.Tests
{
    public class SignalTests
    {
        [Fact]
        public void Signal_Ctor_Success()
        {
            var signal0 = new Signal(true);
            var signal1 = new Signal(false);
        }

        [Fact]
        public void Signal_TurnOnAndWait_CorrectOrder()
        {
            var results = new List<string>();
            var beforeText = "before signal";
            var afterText = "after signal";

            var signal0 = new Signal(false);

            var task0 = Task.Run(() =>
            {
                signal0.WaitForTurnOn();
                results.Add(afterText);
            });

            var task1 = Task.Run(() =>
            {
                results.Add(beforeText);
                signal0.TurnOn();
            });

            task0.Wait();
            task1.Wait();

            Assert.Equal(results[0], beforeText);
            Assert.Equal(results[1], afterText);
        }

        [Fact]
        public void Signal_TurnOffAndWait_CorrectOrder()
        {
            var results = new List<string>();
            var beforeText = "before signal";
            var afterText = "after signal";

            var signal0 = new Signal(true);

            var task0 = Task.Run(() =>
            {
                signal0.TurnOff();
                signal0.WaitForTurnOn();
                results.Add(afterText);
            });

            var task1 = Task.Run(() =>
            {
                results.Add(beforeText);
                signal0.TurnOn();
            });

            task0.Wait();
            task1.Wait();

            Assert.Equal(results[0], beforeText);
            Assert.Equal(results[1], afterText);
        }
        
        [Fact]
        public void Signal_WaitForTurnOn_Timeout()
        {
            var signal = new Signal(false);
            signal.WaitForTurnOn(TimeSpan.FromMilliseconds(100));
        }
    }
}
