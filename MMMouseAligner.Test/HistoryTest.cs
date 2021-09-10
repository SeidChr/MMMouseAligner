using System;
using Xunit;

namespace MMMouseAligner.Test
{
    public class HistoryTest
    {
        [Fact]
        public void SingleElement()
        {
            var history = new History<int>(5);
            history.Enqueue(1);
            Assert.Equal(1, history.Get(1));
            Assert.Equal(1, history.Get(0));
            Assert.Equal(1, history.Get(-1));
        }

        [Fact]
        public void MultiElement()
        {
            var history = new History<int>(5);
            history.Enqueue(1);
            history.Enqueue(2);
            Assert.Equal(1, history.Get(1));
            Assert.Equal(2, history.Get(0));
            Assert.Equal(1, history.Get(-1));
        }

        [Fact]
        public void OverflowGet()
        {
            var history = new History<int>(5);

            history.Enqueue(1);
            history.Enqueue(2);

            Assert.Equal(2, history.Get(4));
            Assert.Equal(1, history.Get(5));

            Assert.Equal(2, history.Get(-4));
            Assert.Equal(1, history.Get(-5));
            Assert.Equal(2, history.Get(-6));
        }

        [Fact]
        public void SizeLimit()
        {
            var history = new History<int>(3);
            
            history.Enqueue(1);
            history.Enqueue(2);
            history.Enqueue(3);
            history.Enqueue(4);

            Assert.Equal(2, history.Get(1));
            Assert.Equal(4, history.Get(0));
            Assert.Equal(3, history.Get(-1));
        }
    }
}
