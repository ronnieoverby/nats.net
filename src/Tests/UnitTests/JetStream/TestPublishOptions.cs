﻿// Copyright 2020 The NATS Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using NATS.Client.Internals;
using NATS.Client.JetStream;
using Xunit;

namespace UnitTests.JetStream
{
    public class TestPublishOptions
    {
        [Fact]
        public void TestDefaultBuilder()
        {
            var po = PublishOptions.Builder().Build();
            Assert.Equal(PublishOptions.DefaultLastSequence, po.ExpectedLastSeq);
            Assert.Equal(PublishOptions.DefaultStream, po.ExpectedStream);
            Assert.Equal(PublishOptions.DefaultTimeout, po.StreamTimeout);
            Assert.Null(po.ExpectedLastMsgId);
            Assert.Null(po.MessageId);
            Assert.Null(po.Stream);
        }

        [Fact]
        public void TestValidBuilderArgs()
        {
            var po = PublishOptions.Builder().
                WithExpectedStream("expectedstream").
                WithExpectedLastMsgId("expectedmsgid").
                WithExpectedLastSequence(42).
                WithMessageId("msgid").
                WithStream("stream").
                WithTimeout(5150).
                Build();

            Assert.Equal("expectedstream", po.ExpectedStream);
            Assert.Equal("expectedmsgid", po.ExpectedLastMsgId);
            Assert.Equal(42, po.ExpectedLastSeq);
            Assert.Equal("msgid", po.MessageId);
            Assert.Equal("stream", po.Stream);
            Assert.Equal(5150, po.StreamTimeout.Millis);

            po = PublishOptions.Builder().
                WithTimeout(Duration.OfMillis(5150)).
                Build();

            Assert.Equal(Duration.OfMillis(5150), po.StreamTimeout);

            // check to allow -
            PublishOptions.Builder().
                WithExpectedStream("test-stream").
                Build();
        }

        [Fact]
        public void TestInvalidBuilderArgs()
        {
            Assert.Throws<ArgumentException>(() => PublishOptions.Builder().
                WithExpectedLastSequence(-1).
                Build());

            Assert.Throws<ArgumentException>(() => PublishOptions.Builder().
                WithMessageId("").
                Build());

            Assert.Throws<ArgumentException>(() => PublishOptions.Builder().
                WithStream("stream.*").
                Build());

            Assert.Throws<ArgumentException>(() => PublishOptions.Builder().
                WithStream("stream.>").
                Build());

            Assert.Throws<ArgumentException>(() => PublishOptions.Builder().
                WithStream("stream.one").
                Build());
        }
    }
}
