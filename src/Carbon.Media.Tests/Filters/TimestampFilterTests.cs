﻿using System;
using Xunit;

namespace Carbon.Media.Processing.Filters.Tests
{
    public class TimeFilterTests
    {
        [Fact]
        public void Parse()
        {
            var filter = TimeFilter.Create(CallSyntax.Parse("time(0.1s)"));

            Assert.Equal(0.1, filter.Value);

            Assert.Equal("time(0.1s)", filter.Canonicalize());
        }

        [Fact]
        public void Parse2()
        {
            var filter = TimeFilter.Create(CallSyntax.Parse("time(110.777333s)"));

            Assert.Equal(110.777333, filter.Value);

            Assert.Equal("time(110.777333s)", filter.Canonicalize());
        }

        [Fact]
        public void Parse3()
        {
            var filter = TimeFilter.Create(CallSyntax.Parse("time(1.345)"));

            Assert.Equal(1.345, filter.Value);

            Assert.Equal("time(1.345s)", filter.Canonicalize());
        }

        [Fact]
        public void Ts()
        {
            Assert.Equal(1d,        TimeSpan.Parse("00:00:01").TotalSeconds);
            Assert.Equal(1.777333d, TimeSpan.Parse("00:00:01.777333").TotalSeconds, 7);
        }
    }
}