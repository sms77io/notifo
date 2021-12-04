﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Diagnostics;
using Notifo.Infrastructure.TestHelpers;
using Xunit;

namespace Notifo.Infrastructure.Json
{
    public class ActivityContextTests
    {
        [Fact]
        public void Should_serialize_and_deserialize_trace_id()
        {
            var sut = ActivityTraceId.CreateRandom();

            var serialized = sut.SerializeAndDeserialize();

            Assert.Equal(sut, serialized);
        }

        [Fact]
        public void Should_serialize_and_deserialize_span_id()
        {
            var sut = ActivitySpanId.CreateRandom();

            var serialized = sut.SerializeAndDeserialize();

            Assert.Equal(sut, serialized);
        }

        [Fact]
        public void Should_serialize_and_deserialize()
        {
            var sut =
                new ActivityContext(
                    ActivityTraceId.CreateRandom(),
                    ActivitySpanId.CreateRandom(),
                    ActivityTraceFlags.Recorded,
                    "State",
                    true);

            var serialized = sut.SerializeAndDeserialize();

            Assert.Equal(sut, serialized);
        }
    }
}